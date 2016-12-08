using System;
using System.Linq;
using System.Text;

using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using System.Net.Http;

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
        
        private async void Battle(kcsapi_battleresult data)
        {
            if (!OntheWay)
            {
                FightID = Guid.NewGuid();
                OntheWay = true;
            }

            using (HttpClient client = new HttpClient())
            {
                // 写记录
                try
                {
                    const string url = "http://smdhz.cf/Api/odata/ShipLogs";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Time: \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "\",");
                    sb.Append("Area: \"" + data.api_quest_name + "\",");
                    sb.Append("Enemy: \"" + data.api_enemy_info.api_deck_name + "\",");
                    sb.Append("Rank: \"" + data.api_win_rank + "\",");
                    sb.Append("Fight: \"" + FightID + "\",");
                    if (data.api_get_ship != null)
                        sb.Append("Drop:\"" + data.api_get_ship.api_ship_name + "\"");
                    else
                        sb.Append("Drop:null");

                    client.DefaultRequestHeaders.Add("Authorization", "BasicAuth 9anofJrJVW7FKNCUaJ3hF");
                    await client.PostAsync(url, new StringContent(
                        "{" + sb.ToString() + "}",
                        Encoding.UTF8,
                        "application/json"));
                }
                catch { }
            }
        }
    }
}
