using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace GameManagerSpace.Hall
{
    public class JoinHandler
    {
        public (Player, InputActionSourceData) AssignNextPlayer(List<InputActionSourceData> _activeController)
        {
            // Get the Rewired Player
            Player rewiredPlayer = FindPlayerWithoutController();

            // Determine which Controller was used to generate the JoinGame Action
            Player systemPlayer = ReInput.players.GetSystemPlayer();
            var inputSources = systemPlayer.GetCurrentInputSources("JoinGame");

            foreach (var source in inputSources)
            {
                if (_activeController.Contains(source)) continue;
                if (source.controllerType == ControllerType.Keyboard || source.controllerType == ControllerType.Joystick)
                {
                    return (rewiredPlayer, source);
                }
                else
                { // Custom Controller
                    throw new System.NotImplementedException();
                }
            }
            return (rewiredPlayer, new InputActionSourceData());
        }
        public Player FindPlayerWithoutController()
        {
            foreach (Player p in ReInput.players.Players)
            {
                if (p.controllers.joystickCount > 0 || p.controllers.hasKeyboard)
                    continue;
                return p;
            }
            return null;
        }
    }

}