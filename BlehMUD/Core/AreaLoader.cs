using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlehMUD.Entities;
using Newtonsoft.Json;

namespace BlehMUD.Core
{
    public class AreaLoader
    {
        public List<Area> LoadAreasFromFolder(string path)
        {
            List<Area> areas = new List<Area>();
            foreach(string filePath in Directory.GetFiles(path, "*.json"))
            {
                Area area = LoadFromFile(filePath);
                areas.Add(area);
            }

            return areas;
        }

        private Area LoadFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string json = sr.ReadToEnd();
                Area area = JsonConvert.DeserializeObject<Area>(json);
                return area;
            }
        }

        public class Area
        {
            public string area_name;
            public List<RoomItem> rooms;
        }

        public class RoomItem
        {
            public string room_id;
            public string name;
            public string description;
            public Dictionary<string, string> exits;

        }
    }
}
