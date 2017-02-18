using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;

namespace Kontur.GameStats.Server
{
    internal class StatServer : IDisposable
    {
        public StatServer()
        {
            listener = new HttpListener();
        }
        
        public void Start(string prefix)
        {
            lock (listener)
            {
                if (!isRunning)
                {
                    listener.Prefixes.Clear();
                    listener.Prefixes.Add(prefix);
                    listener.Start();

                    listenerThread = new Thread(Listen)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.Highest
                    };
                    listenerThread.Start();
                    
                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (listener)
            {
                if (!isRunning)
                    return;

                listener.Stop();

                listenerThread.Abort();
                listenerThread.Join();
                
                isRunning = false;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            Stop();

            listener.Close();
        }
        
        private void Listen()
        {
            while (true)
            {
                try
                {
                    if (listener.IsListening)
                    {
                        var context = listener.GetContext();
                        Task.Run(() => HandleContextAsync(context));

                    }
                    else Thread.Sleep(0);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception error)
                {
                    // TODO: log errors
                }
            }
        }

        private async Task HandleContextAsync(HttpListenerContext listenerContext)
        {
            // TODO: implement request handling
            //Dictionary<string, MemberInfo[]> dictionary = new Dictionary<string, MemberInfo[]>();
            //dictionary.Add(
            //    @"/servers/info", Type.GetType("Kontur.GameStats.Controllers.ServersController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "Info"));
            //dictionary.Add(
            //    @"/servers/[a-zA-Z0-9_]+/info", Type.GetType("Kontur.GameStats.Controllers.ServersController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "Info"));
            //dictionary.Add(
            //    @"/servers/[a-zA-Z0-9_]+/matches/[1-9]\d\d\d-[0-1]\d-\d\dT\d\d:\d\d:\d\dZ",
            //    Type.GetType("Kontur.GameStats.Controllers.ServersController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "Matches"));
            //dictionary.Add(
            //    @"/servers/[a-zA-Z0-9_]+/stats", Type.GetType("Kontur.GameStats.Controllers.ServersController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "Stats"));
            //dictionary.Add(
            //    @"/players/[a-zA-Z0-9_]+/stats", Type.GetType("Kontur.GameStats.Controllers.PlayersController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "Stats"));
            //dictionary.Add(
            //    @"/reports/recent-matches/\d+", Type.GetType("Kontur.GameStats.Controllers.ReportsController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "RecentMatches"));
            //dictionary.Add(
            //    @"/reports/best-players/\d+", Type.GetType("Kontur.GameStats.Controllers.ReportsController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "BestPlayers"));
            //dictionary.Add(
            //    @"/reports/popular-servers/\d+", Type.GetType("Kontur.GameStats.Controllers.ReportsController").FindMembers(
            //        MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (m, n) => m.Name.Equals(n), "PopularServers"));

            string request = listenerContext.Request.Url.AbsolutePath;
            string httpmethod = listenerContext.Request.HttpMethod;

            ControllersCore.Types.RequestMap mapobj = new ControllersCore.Types.RequestMap(@"/{controller:servers}/{param:[a-zA-Z0-9_]+}/{method:matches}/{param:[a-zA-Z0-9_]+}");
            bool val = mapobj.ExecuteMapping(request);



            listenerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            using (var writer = new StreamWriter(listenerContext.Response.OutputStream))
                writer.WriteLine("Hello, world!");

        }

        private static bool DelegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
        {
            // Compare the name of the member function with the filter criteria.
            if (objMemberInfo.Name.ToString() == objSearch.ToString())
            {
                Console.WriteLine("true");
                return true;
            }
            else
            {
                Console.WriteLine("false");
                return false;
            }

        }

        private readonly HttpListener listener;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;
    }
}