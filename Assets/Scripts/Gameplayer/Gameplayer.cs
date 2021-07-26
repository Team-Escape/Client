using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace PlayerSpace.Gameplayer
{
    public class Gameplayer : MonoBehaviour
    {
        [SerializeField] bool testMode = false;
        #region ID Variables
        public int playerID = 0;
        public int teamID = 0;
        #endregion

        #region Classes Variables
        Player input = null;
        Control control = null;
        #endregion

        #region Callbacks
        List<Action<Gameplayer>> gameActions = null;
        public void StartItemCallback() => gameActions[0](this);
        public void CaughtCallBack() => gameActions[1](this);
        public void GoalCallback() => gameActions[2](this);
        #endregion

        #region OuterCall
        public void AssignController(int id)
        {
            playerID = id;
            input = ReInput.players.GetPlayer(playerID);

            bool isKeyboard = input.controllers.joystickCount > 0 ? false : true;
            control.AssignControllerType(isKeyboard);
        }
        public void AssignTeam(int id)
        {
            control.AssignTeam(id);
        }
        #endregion

        #region Unity Native APIs
        private void Awake()
        {
            control = GetComponent<Control>();
        }
        private void OnEnable()
        {
            if (testMode) AssignController(0);
        }
        private void Update()
        {
            ItemHandler();
            MoveHandler();
            CombatHandler();
        }
        #region OnTriggerFuncs
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "PlayerWeapon":
                    Vector2 force = (transform.position - other.transform.parent.position);
                    control.Hurt(force, CaughtCallBack);
                    break;
                case "DashItem":
                    string[] nameSplice = other.name.Split(',');
                    float forceX, forceY = 0;
                    forceX = (string.Compare("n", nameSplice[0]) == 0) ? transform.lossyScale.x : float.Parse(nameSplice[0]);
                    forceY = (string.Compare("n", nameSplice[1]) == 0) ? transform.lossyScale.y : float.Parse(nameSplice[1]);
                    control.DoDash(new Vector2(forceX, forceY));
                    break;
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "StartItem")
            {
                control.ActiveHintUI(true, other.transform.position);

                // Prevent unregister item.
                switch (other.name)
                {
                    case "IceSkate":
                    case "SlimeShoe":
                    case "SwiftnessBoot":
                    case "RocketShoe":
                    case "Shield":
                    case "EnergyDringk":
                    case "ExtralLife":
                    case "Armor":
                    case "InspectorChance":
                    case "DeathWithStronger":
                    case "Balloon":
                        if (input.GetButtonDown("Item"))
                        {
                            control.GetStartItem(other.gameObject, StartItemCallback);
                        }
                        break;
                    default:
                        Debug.Log("Item is not reigstered in gameplayer, name: \'" + other.name + "\'. Make sure you resigister it on both player and control");
                        break;
                }
            }

            if (other.tag == "GameItem")
            {
                Spawner itemControl = other.GetComponent<Spawner>();
                if (itemControl.item == null || control.gameItem != null) return;

                control.ActiveHintUI(true, other.transform.position);
                if (input.GetButtonDown("Item"))
                {
                    control.SetGameItem(itemControl);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "StartItem")
            {
                control.ActiveHintUI(false);
            }
            if (other.tag == "GameItem")
            {
                control.ActiveHintUI(false);
            }
        }
        #endregion
        #endregion

        #region Input Implement
        private void ItemHandler()
        {
            if (input.GetButtonDown("Item"))
            {
                control.UseGameItem();
            }
        }
        private void MoveHandler()
        {
            if (input.GetButton("MoveR"))
            {
                control.Move(1);
            }
            else if (input.GetButton("MoveL"))
            {
                control.Move(-1);
            }
            else if (input.GetButtonUp("MoveL") || input.GetButtonUp("MoveR"))
            {
                control.Move(0);
            }

            if (input.GetButtonDown("Run"))
            {
                control.Run(true);
            }
            else if (input.GetButtonUp("Run"))
            {
                control.Run(false);
            }

            if (input.GetButtonDown("Jump"))
            {
                control.Jump(true);
            }
            else if (input.GetButton("Jump"))
            {
                control.Jump(true);
            }
            else if (input.GetButtonUp("Jump"))
            {
                control.Jump(false);
            }
        }
        public void CombatHandler()
        {
            if (input.GetButtonDown("Attack"))
            {
                control.Attack();
            }
        }
        #endregion
    }
}