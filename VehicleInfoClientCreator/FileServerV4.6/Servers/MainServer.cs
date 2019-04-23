using FileServerV4._6.Sessions;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerV4._6.Servers
{
    public class MainServer : AppServer<MainSession>
    {
        protected override void OnStarted()
        {
            Logger.Info($"{Name}")
            base.OnStarted();
        }     

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }
    }
}
