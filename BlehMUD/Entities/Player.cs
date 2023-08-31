using BlehMUD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Entities
{
    public class Player
    {
        public TcpClient Client { get; set; }
        public Room CurrentRoom { get; set; }
        private EventSystem _eventSystem;
        public int Health { get; set; }
        public string Name { get; set; }
        

        public Player(TcpClient client, Room currentRoom, int health, string name, EventSystem eventSystem)
        {
            Client = client;
            CurrentRoom = currentRoom;
            Health = health;
            Name = name;
            _eventSystem = eventSystem;
        }

        public void EnterRoom(Room room)
        {
            CurrentRoom = room;
            room.AddEntity(this);
            _eventSystem.OnCharacterEnteredRoom(this, room);
        }

        public virtual void ExitRoom()
        {
            if (CurrentRoom != null)
            {
                Room currentRoom = CurrentRoom;
                CurrentRoom.RemoveEntity(this);
                CurrentRoom = null; // To revisit because we need to set it to the next room
                _eventSystem.OnCharacterExitedRoom(this, currentRoom);
            }
        }

        public string GetDescription()
        {
            return "This is you!";
        }
    }
}
