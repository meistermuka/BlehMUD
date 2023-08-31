using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlehMUD.Core;

namespace BlehMUD.Entities
{
    public class NPC
    {
        public Room CurrentRoom { get; set; }
        public string shortDescription { get; set; }
        public string longDescription { get; set; }

        public NPC(string shortDescription, string longDescription)
        {
            
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
        }

        public string GetShortDescription()
        {
            return shortDescription;
        }
        public string GetDescription()
        {
            return $"You see {shortDescription} here";
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
