using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlehMUD.Entities;

namespace BlehMUD.Interfaces
{
    internal interface ICommand
    {
        string Execute(Player player, string[] args);
    }
}
