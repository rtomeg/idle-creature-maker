using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerSlotsUtils
{
    public static class PlayerSlotsExtension
    
    {
        public static bool GetEmptySlot(this List<PlayerSlot> playerSlots, out PlayerSlot playerSlot)
        {
            playerSlot = playerSlots.Find(x => x.isFree);
            return playerSlot != null;
        }

        public static bool IsPlayerInGame(this List<PlayerSlot> playerSlots, string username)
        {
            return playerSlots.Any(x => x.username.Equals(username));
        }

        public static bool ExistsEmptySlot(this List<PlayerSlot> playerSlots)
        {
            return playerSlots.Any(x => x.isFree);
        }

        public static bool GetPlayerSlotByName(this List<PlayerSlot> playerSlots, out PlayerSlot playerSlot, string username)
        {
            playerSlot = playerSlots.Find(x => x.username.Equals(username));
            return playerSlot != null;
        }
        
        public static bool GetPlayerSlotByName(this List<PlayerSlot> playerSlots, string username)
        {
            return playerSlots.Any(x => username.Equals(x.username));
        }
    }

}
    
