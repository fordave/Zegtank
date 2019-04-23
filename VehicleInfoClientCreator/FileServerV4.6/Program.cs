using FileServerV4._6.Servers;
using FileServerV4._6.Sessions;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerV4._6
{
    class Program
    {
        static void Main(string[] args)
        {   
            var appServer = new MainServer();
            //Setup the appServer
            if (!appServer.Setup(20120)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }
            appServer.NewSessionConnected += new SessionHandler<MainSession>(AppServer_NewSessionConnected);
            appServer.SessionClosed += AppServer_SessionClosed;
            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("The server started successfully, press key 'q' to stop it!");
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        private static void AppServer_SessionClosed(MainSession session, CloseReason value)
        {
           
        }

        private static void AppServer_NewSessionConnected(MainSession session)
        {
            session.Send("Welcome to SuperSocket Telnet Server");
        }

        private static void AppServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            switch (requestInfo.Key.ToUpper())
            {
                case ("ECHO"):
                    session.Send(requestInfo.Body);
                    break;

                case ("ADD"):
                    session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
                    break;

                case ("MULT"):

                    var result = 1;

                    foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                    {
                        result *= factor;
                    }

                    session.Send(result.ToString());
                    break;
            }
        }
    }
}
