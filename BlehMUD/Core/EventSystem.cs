using BlehMUD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Core
{
    public class EventSystem
    {
        public event Action<Player, Room> PlayerEnteredRoom;
        public event Action<Player, Room> PlayerExitedRoom;

        public void OnCharacterEnteredRoom(Player character, Room room)
        {
            PlayerEnteredRoom?.Invoke(character, room);
        }

        public void OnCharacterExitedRoom(Player character, Room room)
        {
            PlayerExitedRoom?.Invoke(character, room);
        }
    }

}
