using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using Grabacr07.KanColleWrapper.Models;
using System.ComponentModel;

namespace Logger
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(KanColleProxy proxy)
        {
            IsOnline = db.DatabaseExists();

            proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => Battle(x.Data));
            proxy.api_port.TryParse<kcsapi_port>().Subscribe(x => OntheWay = false);
        }

        private bool OntheWay = false;
        private Guid FightID = Guid.Empty;
        private LogDataContext db = new LogDataContext();

        public bool IsOnline { get; private set; }
        public bool IsOffline { get { return !IsOnline; } }
        public ShipLog LastLog { get; private set; }

        private void Battle(kcsapi_battleresult data)
        {
            if (!OntheWay)
            {
                FightID = Guid.NewGuid();
                OntheWay = true;
            }

            // 准备数据
            LastLog = new ShipLog()
            {
                Time = DateTime.Now,
                Area = data.api_quest_name,
                Enemy = data.api_enemy_info.api_deck_name,
                Rank = data.api_win_rank.First(),
                Fight = FightID
            };
            if (data.api_get_ship != null)
                LastLog.Drop = data.api_get_ship.api_ship_name;
            PropertyChanged(this, new PropertyChangedEventArgs("LastLog"));

            // 写记录
            if (IsOnline)
            {
                db.ShipLog.InsertOnSubmit(LastLog);
                db.SubmitChanges();
            }
            else
                using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\ShipLog.csv", FileMode.Append))
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff},{1},{2},{3},{4},{{{5}}}",
                        LastLog.Time,
                        LastLog.Area,
                        LastLog.Enemy,
                        LastLog.Rank,
                        LastLog.Drop,
                        LastLog.Fight);
                }
        }

        public event PropertyChangedEventHandler PropertyChanged = (se, ev) => { };
    }
}
