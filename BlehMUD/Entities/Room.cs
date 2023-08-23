using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Entities
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Room> ConnectedRooms { get; } = new Dictionary<string, Room>();
        public List<NPC> NPCs { get; } = new List<NPC>();

        public string GetFullDescription()
        {
            StringBuilder fullDescription = new StringBuilder();
            fullDescription.AppendLine($"You are in {Name}");
            fullDescription.AppendLine(Description);
            return fullDescription.ToString();
        }
    }
}
