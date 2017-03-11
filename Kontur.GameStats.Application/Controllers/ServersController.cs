using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Kontur.GameStats.Application.Core;
using Kontur.GameStats.Application.App_Data;
using Kontur.GameStats.Application.Models;


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

            var servers_gamemodes = dbcontext.Servers.Select(a => a).Join(
            dbcontext.ServerGameModes.Select(a => a), sr => sr.serverId, gm => gm.serverId, (sr, gm) => new
            {
                endpoint = sr.endpoint,
                name = sr.name,
                gameMode = gm.gamemode
            });

            var groupedByServerId = servers_gamemodes.GroupBy(g => g.endpoint);

            foreach (var servergroup in groupedByServerId)
            {
                var game_modes = servergroup.Select(s => s.gameMode).ToList();
                var server_name = servergroup.Select(s => s.name).FirstOrDefault();

                serversInfo.Add(new ServersInfoModel()
                {
                    Endpoint = servergroup.Key,
                    Info = new ServerInfoModel()
                    {
                        Name = server_name,
                        GameModes = game_modes
                    }
                });
            }

            return serversInfo;
        }

        [Method(HttpMethods.GET)]
        [Name("info")]
        public object Info(string endpoint)
        {
            ServerInfoModel serverInfo = new ServerInfoModel();

            var dbContect = dbcontext;
            var isexist = dbContect.Servers.Any(s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
            if (!isexist)
                return HttpStatusCode.NotFound;

            var exist_server = dbContect.Servers.FirstOrDefault(
            s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));

            var game_modes = dbContect.ServerGameModes.Where(
            gm => gm.serverId.Equals(exist_server.serverId)).Select(gm => gm.gamemode).ToList();

            serverInfo.Name = exist_server.name;
            serverInfo.GameModes = game_modes;

            return serverInfo;
        }

        [Method(HttpMethods.PUT)]
        [Name("info")]
        public HttpStatusCode Info(string endpoint, ServerInfoModel data)
        {
            var dbContect = dbcontext;
            var isexist = dbContect.Servers.Any(s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
            if (isexist)
            {
                var exist_server = dbContect.Servers.FirstOrDefault(s => s.endpoint.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
                exist_server.name = data.Name;
                dbContect.Entry<Servers>(exist_server).State = System.Data.Entity.EntityState.Modified;

                var game_modes = dbContect.ServerStatGameModes.Where(g => g.serverId.Equals(exist_server.serverId));
                foreach (var gm in game_modes)
                {
                    dbContect.Entry<ServerStatGameModes>(gm).State = System.Data.Entity.EntityState.Deleted;
                }
                dbContect.SaveChanges();
                List<ServerGameModes> new_game_modes = new List<ServerGameModes>();
                foreach (var gm in data.GameModes)
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
                if (servers.Count > 0 && servers.Last() != null)
                {
                    server_id = servers.Last().serverId + 1;
                }
                dbContect.Servers.Add(new Servers()
                {
                    endpoint = endpoint,
                    serverId = server_id,
                    name = data.Name
                });

                List<ServerGameModes> game_modes = new List<ServerGameModes>();
                foreach (var gm in data.GameModes)
                {
                    game_modes.Add(new ServerGameModes()
                    {
                        serverId = server_id,
                        gamemode = gm
                    });
                }
                dbContect.ServerGameModes.AddRange(game_modes);
            }
            dbContect.SaveChangesAsync();
            return System.Net.HttpStatusCode.OK;
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
            try
            {
                var dbcontext = new ConturDataModel();
                //Проверка существования сервера
                var isserver = dbcontext.Servers.Any(s => s.endpoint.Equals(endpoint));
                if (!isserver)
                    return HttpStatusCode.BadRequest;
                var exist_server = dbcontext.Servers.Where(s => s.endpoint.Equals(endpoint)).First();

                //Определим match_id
                var match_id = dbcontext.Matches.Count();

                var PlayersList = dbcontext.Players.ToList();
                var ModifiedScoreBoardCollection = PlayersList.Join(data.ScoreBoadrd, p => p.name, s => s.Name,
                (p, s) => new MatchesPlayers() { playerId = p.playerId, deaths = s.Deaths, frags = s.Frags, kills = s.Kills, matchId = match_id }).ToList();
                //Добавление нового матча
                dbcontext.Matches.Add(new Matches()
                {
                    matchId = match_id,
                    serverId = exist_server.serverId,
                    timestamp = timestamp,
                    fraglimit = data.FragLimit,
                    timelimit = data.TimeLimit,
                    timeelapsed = data.TimeElapsed,
                    gamemode = data.GameMode,
                    map = data.Map
                });
                //Формирование статистики по игрокам для для матча
                int pos = 1;
                foreach (var playerScore in ModifiedScoreBoardCollection)
                {
                    playerScore.scoreboardPersent = (ModifiedScoreBoardCollection.Count() - pos) / (ModifiedScoreBoardCollection.Count() - 1) * 100;
                    pos++;
                }
                //Добавление статистики игроков матча 
                dbcontext.MatchesPlayers.AddRange(ModifiedScoreBoardCollection);

                dbcontext.SaveChangesAsync();
            }
            catch
            {

            }            
            return HttpStatusCode.OK;
        }

        [Method(HttpMethods.GET)]
        [Name("stats")]
        public string Stats(string endpoint)
        {
            return "public string Stats(string endpoint)";
        }

    }
}

