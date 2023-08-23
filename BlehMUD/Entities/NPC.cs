using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Entities
{
    public class NPC : Character
    {
        public string GetDescription()
        {
            return $"You see {Name} here";
        }

        public string GetDialogue()
        {
            return "Greetings stranger!";
        }

        public void MoveTo()
        {
        }
    }
}
