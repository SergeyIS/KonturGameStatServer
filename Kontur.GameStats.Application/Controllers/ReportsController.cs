using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Application.Core;

namespace Kontur.GameStats.Application.Controllers
{
    [Name("reports")]
    class ReportsController
    {
       [Method(HttpMethods.GET)][Name("recent-matches")]
        public string RecentMatches(int count)
        {
            return "public string RecentMatches(int count)";
        }

        

        [Method(HttpMethods.GET)][Name("best-players")]
        public string BestPlayers(int count)
        {
            return "public string BestPlayers(int count)";
        }

        

        [Method(HttpMethods.GET)][Name("popular-servers")]
        public string PopularServers(int count)
        {
            return "public string PopularServers(int count)";
        }




        /*
            Методы заглушки для поддержания 
            api: reports/XXXX-XXXX[/<count>] с необязательным параметром count
        */

        [Method(HttpMethods.GET)][Name("popular-servers")]
        public void PopularServers()
        {
            PopularServers(5);
        }

        [Method(HttpMethods.GET)][Name("best-players")]
        public void BestPlayers()
        {
            BestPlayers(5);
        }

        [Method(HttpMethods.GET)][Name("recent-matches")]
        public void RecentMatches()
        {
            RecentMatches(5);
        }
    }
}
