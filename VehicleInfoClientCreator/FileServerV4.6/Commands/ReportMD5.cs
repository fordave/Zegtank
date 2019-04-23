using FileServerV4._6.Sessions;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerV4._6.Commands
{
    public class ReportMD5 : CommandBase<MainSession, StringRequestInfo>
    {
        public override void ExecuteCommand(MainSession session, StringRequestInfo requestInfo)
        {
            //TODO:记录连接信息,并返回文件MD码
            session.Send("test reportmd5");
        }
    }
}
