using BlehMUD.Interfaces;
using BlehMUD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Commands
{
    public class QuitCommand : ICommand
    {
        public QuitCommand() 
        {
            Console.WriteLine($"Registering {GetType().Name}");
        }

        public string Execute(Player player, string[] args )
        {
            return "Disconnected from server.";
        }
    }
}
