﻿using BlehMUD.Interfaces;
using BlehMUD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Commands
{
    public class LookCommand : ICommand
    {
        public LookCommand() 
        { 
            Console.WriteLine($"Registering {GetType().Name}");
        }
        public string Execute(Player player, string[] args)
        {
            if (args.Length == 0 )
            {
                return "You look around\r\n";
            }
            else
            {
                return "something else";
            }
            
        }
    }
}
