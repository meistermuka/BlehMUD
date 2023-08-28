using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Commands
{
    public static class CommandType
    {
        
        public static bool IsTypeDirection(string command)
        {
            string[] directionType = { "west", "east", "north", "south", "up", "down" };
            return directionType.Contains(command);
        }
    }
}
