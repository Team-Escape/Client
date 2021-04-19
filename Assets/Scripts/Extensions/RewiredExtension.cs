using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace GameManagerSpace
{
    public static class RewiredExtension
    {
        public static List<Player> FindAllPlayersWithJoystick(this List<Player> inputs)
        {
            IList<Player> players = ReInput.players.Players;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].controllers.joystickCount <= 0
                && !players[i].controllers.hasKeyboard)
                    continue;
                inputs.Add(players[i]);
            }
            return inputs;
        }
        public static Player FindPlayerWithoutJoystick(this IList<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].controllers.joystickCount > 0
                || players[i].controllers.hasKeyboard) continue;
                return players[i];
            }
            return null;
        }
        public static void SelectAllTheMap(this List<Player> inputs, string mapName)
        {
            foreach (Player p in inputs)
            {
                p.controllers.maps.SetAllMapsEnabled(false);
                p.controllers.maps.SetMapsEnabled(true, mapName);
            }
        }
        public static Player SelectTheMap(this Player player, string mapName)
        {
            Player p = player;
            p.controllers.maps.SetAllMapsEnabled(false);
            p.controllers.maps.SetMapsEnabled(true, mapName);
            return p;
        }
        public static List<Player> GetActivePlayers(this IList<Player> players)
        {
            List<Player> ps = new List<Player>();
            foreach (Player p in players)
            {
                if (p.name == "System") continue;
                if (p.isPlaying) ps.Add(p);
            }
            return ps;
        }
    }
}