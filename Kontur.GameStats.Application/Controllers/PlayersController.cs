using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Kontur.GameStats.Application.Core;
using Kontur.GameStats.SharedResources;
using Kontur.GameStats.Data;


namespace Kontur.GameStats.Application.Controllers
{
    [Name("players")]
    class PlayersController
    {
        [Method(HttpMethods.GET)][Name("stats")]
        public object Stats(string name)
        {
            var playerStats = new PlayerStatsModel();

            using(var dbmanager = new DataManager())
            {
                playerStats = dbmanager.GetPlayerStats(name);

                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.BadRequest;
            }

            return playerStats;
        }
    }
}
