using BlehMUD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Entities
{
    public class Character
    {
        private EventSystem _eventSystem;
        public string Name { get; set; }
        public int Health { get; set; }

        public Room CurrentRoom { get; set; }

        public Character(string name, EventSystem eventSystem)
        {
            Name = name;
            _eventSystem = eventSystem;
        }

        public virtual string GetDescription()
        {
            return $"You see {Name} here";
        }
    }

    
}
