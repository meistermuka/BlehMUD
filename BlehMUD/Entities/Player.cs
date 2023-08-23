using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Entities
{
    public class Player : Character
    {
        public TcpClient Client { get; set; }
        public Room CurrentRoom { get; set; }

        public override string GetDescription()
        {
            return "This is you!";
        }
    }
}
