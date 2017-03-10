using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Application.Core;
using Kontur.GameStats.Application.App_Data;
using Kontur.GameStats.Application.Models;
using System.Net;

namespace Kontur.GameStats.Application.Controllers
{
    [Name("servers")]
    class ServersController : BaseController
    {
        [Method(HttpMethods.GET)][Name("info")]
        public object Info()
        {
            var serversInfo = new List<ServersInfoModel>();

            var servers_gamemodes = ConturDbContext.Servers.Select(a=>a).Join(
                ConturDbContext.ServerGameModes.Select(a => a), sr => sr.serverId, gm => gm.serverId, (sr, gm) => new {
                    endpoint = sr.endpoint,
                    name = sr.name,
                    gameMode = gm.gamemode
                });

            var groupedByServerId = servers_gamemodes.GroupBy(g=>g.endpoint);

            foreach (var servergroup in groupedByServerId)
            {
                var game_modes = servergroup.Select(s=>s.gameMode).ToList();
                var server_name = servergroup.Select(s => s.name).FirstOrDefault();

                serversInfo.Add(new ServersInfoModel() {
                    endpoint = servergroup.Key,
                    info = new ServerInfoModel()
                    {
                        name = server_name,
                        gameModes = game_modes
                    }
                });
            }

            return serversInfo;
        }

        [Method(HttpMethods.GET)][Name("info")]
        public object Info(string endpoint)
        {
            ServerInfoModel serverInfo = new ServerInfoModel();

            var dbContect = ConturDbContext;
            var isexist = dbContect.Servers.Any(s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
            if (!isexist)
                return HttpStatusCode.NotFound;

            var exist_server = dbContect.Servers.FirstOrDefault(
                s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));

            var game_modes = dbContect.ServerGameModes.Where(
                gm => gm.serverId.Equals(exist_server.serverId)).Select(gm => gm.gamemode).ToList();

            serverInfo.name = exist_server.name;
            serverInfo.gameModes = game_modes;

            return serverInfo;
        }

        [Method(HttpMethods.PUT)][Name("info")]
        public HttpStatusCode Info(string endpoint, ServerInfoModel data)
        {
            var dbContect = ConturDbContext;
            var isexist = dbContect.Servers.Any(s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
            if (isexist)
            {
                var exist_server = dbContect.Servers.FirstOrDefault(s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
                exist_server.name = data.name;
                dbContect.Entry<Servers>(exist_server).State = System.Data.Entity.EntityState.Modified;

                var game_modes = dbContect.ServerStatGameModes.Where(g => g.serverId.Equals(exist_server.serverId));
                foreach (var gm in game_modes)
                {
                    dbContect.Entry<ServerStatGameModes>(gm).State = System.Data.Entity.EntityState.Deleted;
                }
                dbContect.SaveChanges();
                List<ServerGameModes> new_game_modes = new List<ServerGameModes>();
                foreach (var gm in data.gameModes)
                {
                    new_game_modes.Add(new ServerGameModes()
                    {
                        serverId = exist_server.serverId,
                        gamemode = gm
                    });
                }
                dbContect.ServerGameModes.AddRange(new_game_modes);         
            }
            else
            {
                var servers = dbContect.Servers.ToList();
                int server_id = 0;
                if (servers.Count>0 && servers.Last() != null)
                {
                    server_id = servers.Last().serverId + 1;
                }
                dbContect.Servers.Add(new Servers() {
                    endpoint = endpoint,
                    serverId = server_id,
                    name = data.name
                });

                List<ServerGameModes> game_modes = new List<ServerGameModes>();
                foreach (var gm in data.gameModes)
                {
                    game_modes.Add(new ServerGameModes() {
                        serverId = server_id,
                        gamemode = gm
                    });
                }
                dbContect.ServerGameModes.AddRange(game_modes);
            }
            dbContect.SaveChangesAsync();
            return System.Net.HttpStatusCode.OK;
        }


        [Method(HttpMethods.GET)][Name("matches")]
        public string Matches(string endpoint, DateTime timestamp)
        {
            return "public string Matches(string endpoin, DateTime timestamp)";
        }

        [Method(HttpMethods.PUT)][Name("matches")]
        public string Matches(string endpoint, DateTime timestamp, string data)
        {
            return "public string Matches(string endpoin, DateTime timestamp, string data)";
        }

        [Method(HttpMethods.GET)][Name("stats")]
        public string Stats(string endpoint)
        {
            return "public string Stats(string endpoint)";
        }
        
    }
}

