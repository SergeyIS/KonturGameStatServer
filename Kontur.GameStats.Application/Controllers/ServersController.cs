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
                var isSuccessful = dbmanager.GetServerInfo(endpoint, serverInfo);
                if (!isSuccessful)
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
        public string Matches(string endpoint, DateTime timestamp)
        {
            return "public string Matches(string endpoin, DateTime timestamp)";
        }

        [Method(HttpMethods.PUT)]
        [Name("matches")]
        public HttpStatusCode Matches(string endpoint, DateTime timestamp, ServerMatchesModel data)
        {
            using(var dbmanager = new DataManager())
            {
                dbmanager.OpenConnetion();
                var isSuccessful = dbmanager.AddMatch(endpoint, timestamp, data);

                if (isSuccessful)
                    return HttpStatusCode.OK;

                return HttpStatusCode.BadRequest;
            }     
        }

        [Method(HttpMethods.GET)]
        [Name("stats")]
        public string Stats(string endpoint)
        {
            return "public string Stats(string endpoint)";
        }

    }
}

