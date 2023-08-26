using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlehMUD.Entities;

namespace BlehMUD.Core
{
    public class RoomManager
    {
        private Dictionary<string, Room> _rooms = new Dictionary<string, Room>();

        public void AddRoom(Room room)
        {
            _rooms[room.Name.ToLower()] = room;
        }

        public Room GetRoomByName(string name)
        {
            _rooms.TryGetValue(name.ToLower(), out Room room);
            return room;
        }
    }
}
