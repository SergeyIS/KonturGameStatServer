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
    [Name("servers")]
    class ServersController : BaseController
    {
        [Method(HttpMethods.GET)]
        [Name("info")]
        public object Info()
        {
            var serversInfo = new List<ServersInfoModel>();
            using(var dbmanager = new DataManager())
            {
                dbmanager.OpenConnetion();
                serversInfo = dbmanager.GetServersInfo();
            }
            return serversInfo;
        }

        [Method(HttpMethods.GET)]
        [Name("info")]
        public object Info(string endpoint)
        {
            ServerInfoModel serverInfo = new ServerInfoModel();
            using(var dbmanager = new DataManager())
            {
                serverInfo = dbmanager.GetServerInfo(endpoint);
                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.BadRequest;
            }
            return serverInfo;
        }

        [Method(HttpMethods.PUT)]
        [Name("info")]
        public HttpStatusCode Info(string endpoint, ServerInfoModel data)
        {
            using (var dbmanager = new DataManager())
            {
                dbmanager.OpenConnetion();
                dbmanager.AddServer(endpoint, data.Name, data.GameModes);
            }
            return HttpStatusCode.OK;
        }


        [Method(HttpMethods.GET)]
        [Name("matches")]
        public object Matches(string endpoint, DateTime timestamp)
        {
            using(var dbmanager = new DataManager())
            {
                dbmanager.OpenConnetion();
                var matchinfo = dbmanager.GetMatchInfo(endpoint, timestamp);
                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.NotFound;
                return matchinfo;
            }
        }

        [Method(HttpMethods.PUT)]
        [Name("matches")]
        public HttpStatusCode Matches(string endpoint, DateTime timestamp, ServerMatchesModel data)
        {
            using(var dbmanager = new DataManager())
            {
                dbmanager.OpenConnetion();
                dbmanager.AddMatch(endpoint, timestamp, data);

                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.BadRequest;

                //Протестировано//dbmanager.UpdateServersStats(endpoint);//Обновляет статистические данные о сервере из глобальной таблицы.
                dbmanager.UpdatePlayersStats(endpoint, data);//Обновляет статистические данные об игроке из глобальной таблицы

                //Протестировано//dbmanager.UpdateServerMatchesStats(endpoint, data);//Обновляет статистические данные о сервере (режимы игры, карты)
                //Протестировано//dbmanager.UpdatePlayersMatchesStats(endpoint, data);//Обновляет статистические данные об игроке (режим игры, сервер, итд)

                return HttpStatusCode.OK;
            }     
        }

        [Method(HttpMethods.GET)]
        [Name("stats")]
        public object Stats(string endpoint)
        {
            var serverStats = new ServerStatsModel();

            using(var dbmanager = new DataManager())
            {
                dbmanager.OpenConnetion();
                serverStats = dbmanager.GetServerStat(endpoint);
                if (dbmanager.StateOfCurrentOperation == OperationState.Failed)
                    return HttpStatusCode.NotFound;
            }
            return serverStats;
        }

    }
}

