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

namespace Logger
{
    public class Counter
    {
        public Counter(KanColleProxy proxy)
        {
            proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => Battle(x.Data));
            proxy.api_port.TryParse<kcsapi_port>().Subscribe(x => Port(x.Data));
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

            // 记录
            using (LogDataContext db = new LogDataContext())
            {
                ShipLog log = new ShipLog()
                {
                    Time = DateTime.Now,
                    Area = data.api_quest_name,
                    Enemy = data.api_enemy_info.api_deck_name,
                    Rank = data.api_win_rank,
                    Fight = FightID
                };
                if (data.api_get_ship != null)
                    log.Drop = data.api_get_ship.api_ship_name;
                db.ShipLog.InsertOnSubmit(log);
                db.SubmitChanges();
            }
        }

        private void Port(kcsapi_port data)
        {
            OntheWay = false;
        }
    }
}
