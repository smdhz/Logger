﻿using System;
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
            IsOnline = new LogDataContext().DatabaseExists();
            if (IsOnline)
            {
                proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => Battle(x.Data));
                proxy.api_port.TryParse<kcsapi_port>().Subscribe(x => OntheWay = false);
            }
        }

        private bool OntheWay = false;
        private Guid FightID = Guid.Empty;

        public bool IsOnline { get; private set; }
        public bool IsOffline { get { return !IsOnline; } }
        public ShipLog LastLog { get; private set; }

        private void Battle(kcsapi_battleresult data)
        {
            if (!IsOnline)
                return;

            if (!OntheWay)
            {
                FightID = Guid.NewGuid();
                OntheWay = true;                    
            }

            // 记录
            using (LogDataContext db = new LogDataContext())
            {
                LastLog = new ShipLog()
                {
                    Time = DateTime.Now,
                    Area = data.api_quest_name,
                    Enemy = data.api_enemy_info.api_deck_name,
                    Rank = data.api_win_rank,
                    Fight = FightID
                };
                if (data.api_get_ship != null)
                    LastLog.Drop = data.api_get_ship.api_ship_name;
                PropertyChanged(this, new PropertyChangedEventArgs("LastLog"));
                db.ShipLog.InsertOnSubmit(LastLog);
                db.SubmitChanges();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (se, ev) => { };
    }
}
