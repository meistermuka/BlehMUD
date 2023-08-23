using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Entities
{
    public class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }

        public virtual string GetDescription()
        {
            return $"You see {Name} here";
        }
    }

    
}
