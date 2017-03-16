using System;
using System.Collections.Generic;
using System.Net;
using Kontur.GameStats.Application.Core;
using Kontur.GameStats.SharedResources;
using Kontur.GameStats.Data;

namespace Kontur.GameStats.Application.Controllers
{
    [Name("reports")]
    class ReportsController
    {
       [Method(HttpMethods.GET)][Name("recent-matches")]
        public object RecentMatches(int count)
        {
            var resentMatches = new List<RecentMatchesModel>();

            if (count <= 0)
            {
                count = 5;
            }
            else if (count > 50)
            {
                count = 50;
            }

            using (var dbmanager = new DataManager())
            {
                resentMatches = dbmanager.GetRecentMatches(count);
                if(dbmanager.StateOfCurrentOperation == OperationState.Failed)
                {
                    return HttpStatusCode.BadRequest;
                }
            }
            return resentMatches;
        }

        

        [Method(HttpMethods.GET)][Name("best-players")]
        public object BestPlayers(int count)
        {
            var bestPlayers = new List<BestPlayersModel>();
            if(count <= 0)
            {
                count = 5;
            }
            else if (count > 50)
            {
                count = 50;
            }
            
            using(var dbmanager = new DataManager())
            {
                bestPlayers = dbmanager.GetBestPlayers(count);
                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.BadRequest;
            }
            return bestPlayers;
        }

        

        [Method(HttpMethods.GET)][Name("popular-servers")]
        public object PopularServers(int count)
        {
            var popularServers = new List<PopularServersModel>();

            if (count <= 0)
            {
                count = 5;
            }
            else if (count > 50)
            {
                count = 50;
            }

            using (var dbmanager = new DataManager())
            {
                popularServers = dbmanager.GetPopularServers(count);
                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.BadRequest;
            }
            return popularServers;
        }




        /*
            Методы заглушки для поддержания 
            api: reports/XXXX-XXXX[/<count>] с необязательным параметром count
        */

        [Method(HttpMethods.GET)][Name("popular-servers")]
        public void PopularServers()
        {
            this.PopularServers(5);
        }

        [Method(HttpMethods.GET)][Name("best-players")]
        public void BestPlayers()
        {
            this.BestPlayers(5);
        }

        [Method(HttpMethods.GET)][Name("recent-matches")]
        public void RecentMatches()
        {
            this.RecentMatches(5);
        }
    }
}
