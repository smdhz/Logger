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
using System.Net.Http;
using System.Runtime.Serialization.Json;

namespace Logger
{
    public class Counter
    {
        public Counter(KanColleProxy proxy)
        {
            proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => Battle(x.Data));
            proxy.api_req_combined_battle_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => Battle(x.Data));
            proxy.api_port.TryParse<kcsapi_port>().Subscribe(x => OntheWay = false);
        }

        private bool OntheWay = false;
        private Guid FightID = Guid.Empty;
        
        private void Battle(kcsapi_battleresult data)
        {
            if (!OntheWay)
            {
                FightID = Guid.NewGuid();
                OntheWay = true;
            }
            
            // 准备数据
            var record = new
            {
                Time = DateTime.Now,
                Area = data.api_quest_name,
                Enemy = data.api_enemy_info.api_deck_name,
                Rank = data.api_win_rank.First(),
                Fight = FightID,
                Drop = data.api_get_ship?.api_ship_name
            };

            using (HttpClient client = new HttpClient())
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer se = new DataContractJsonSerializer(record.GetType());
                // 写记录
                //client.PutAsync(
                //    "http://smdhz.cf/Api/odata/ShipLogs",
                //    new StringContent(se.));
            }
        }
    }
}
