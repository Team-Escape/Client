using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Cinemachine;

namespace PlayerSpace.Gameplayer
{
    public class Gameplayer : MonoBehaviour
    {
        [SerializeField] bool testMode = false;
        #region ID Variables
        public int playerID = 0;
        public int teamID = 0;
        public int currentRoomID = 0;
        #endregion

        #region Classes Variables
        Player input = null;
        Control control = null;
        #endregion

        #region Callbacks
        List<Action<Gameplayer>> gameActions = null;
        void StartItemCallback() => gameActions[0](this);
        void CaughtCallBack() => gameActions[1](this);
        void GoalCallback() => gameActions[2](this);

        List<Action<Gameplayer, CinemachineConfiner>> changeLevel = null;
        void GoNextRoom() => changeLevel[0](this, control.GetConfiner());
        void GoPrevRoom() => changeLevel[1](this, control.GetConfiner());
        bool isTeleporting = false;
        #endregion

        #region OuterCall
        public void AssignController(int id)
        {
            playerID = id;
            input = ReInput.players.GetPlayer(playerID);

            bool isKeyboard = input.controllers.joystickCount > 0 ? false : true;
            control.AssignControllerType(isKeyboard);
        }
        public void AssignTeam(int id, List<Action<Gameplayer>> callbacks, List<System.Action<Gameplayer, CinemachineConfiner>> changeLevelCallbacks)
        {
            control.AssignTeam(id);
            gameActions = callbacks;
            changeLevel = changeLevelCallbacks;
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
                case "RoomTeleport":
                    if (changeLevel == null || isTeleporting) return;
                    isTeleporting = true;
                    switch (other.name)
                    {
                        case "NextCollider":
                            GoNextRoom();
                            break;
                        case "PrevCollider":
                            GoPrevRoom();
                            break;
                    }
                    this.AbleToDo(0.1f, () => isTeleporting = false);
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
                Spawner spawner = other.GetComponent<Spawner>();
                if (spawner.currentItemID == -1 || control.IsItemNull() == false)
                    return;

                control.ActiveHintUI(true, other.transform.position);

                if (input.GetButtonDown("Item"))
                {
                    control.SetGameItem(spawner.currentItemID, spawner.ResetItem);
                }
            }

            if (other.tag == "Flag")
            {
                if (control.IsGoaled()) return;

                control.ActiveHintUI(true, other.transform.position);
                if (input.GetButtonDown("Item"))
                {
                    control.ActiveHintUI(false);
                    control.Goal(GoalCallback);
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
            if (other.tag == "Flag")
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