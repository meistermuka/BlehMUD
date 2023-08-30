using BlehMUD.Interfaces;
using BlehMUD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Commands
{
    public class MoveCommand : ICommand
    {
        public MoveCommand() 
        {
            Console.WriteLine($"Registering {GetType().Name}");
        }

        public string Execute(Player player, string[] args)
        {
            return MovePlayer(player, args[0]);
        }

        private string MovePlayer(Player player, string direction)
        {
            if (player.CurrentRoom.Exits.TryGetValue(direction.ToLower(), out Room newRoom))
            {
                player.CurrentRoom = newRoom;
                return player.CurrentRoom.Description;
            }

            return "You cannot go that way!";
        }
    }
}
