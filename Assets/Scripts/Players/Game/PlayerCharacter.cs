using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Cinemachine;
using GameManagerSpace;

namespace PlayerSpace.Game
{
    public class PlayerCharacter : MonoBehaviour
    {
        public Player GetPlayer { get { return input; } }

        [SerializeField] bool testMode = false;
        [SerializeField] CinemachineConfiner confiner = null;
        Player input = null;
        Control control = null;
        ExecutionManager executionManager = null;

        List<System.Action<PlayerCharacter>> gameActions = null;
        List<System.Action<PlayerCharacter, CinemachineConfiner>> changeLevel = null;
        public int playerId = 0;
        public int teamId = 0;
        public int currentRoomId = 0;
        bool isTeleporting = false;

        public void AssignController(int id)
        {
            playerId = id;
            input = ReInput.players.GetPlayer(playerId);
            SetCamera();
        }

        void SetCamera()
        {
            LayerMask layer = LayerMask.NameToLayer("P" + (playerId + 1) + "Cam");
            Camera camera = transform.parent.GetComponentInChildren<Camera>();

            // Open the layer of layer + playerId
            camera.cullingMask |= 1 << layer;

            // Change cinemachine camera child object layer
            Transform follow = transform.parent.GetChild(2);
            follow.gameObject.layer = layer;
        }

        public void AssignTeam(int id, List<System.Action<PlayerCharacter>> callbacks, List<System.Action<PlayerCharacter, CinemachineConfiner>> changeLevelCallbacks)
        {
            teamId = id;
            gameActions = callbacks;
            changeLevel = changeLevelCallbacks;
            control.GameSetup(id);
        }

        public void DevInput()
        {
            /* Remember to remove before deployment */
            if (Input.GetKeyDown(KeyCode.H)) control.Hurt(new Vector2(transform.localScale.x * 10, 10f));
            if (Input.GetKeyDown(KeyCode.C)) control.CancelItem();
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Bigger");
                control.SizeAdjust(1.25f);
            }
            if (Input.GetKeyDown(KeyCode.V)) control.SizeAdjust(0.8f);

            if (Input.GetKeyDown(KeyCode.Q)) { control.Mutate(); }
        }

        public void MoveInput()
        {
            float movement = input.GetAxis("Move Horizontal");
            control.Move(movement);

            if (input.GetButtonDown("Jump")) control.Jump(true);
            else if (input.GetButtonUp("Jump")) control.Jump(false);

            if (input.GetButtonDown("Run")) control.Run(true);
            else if (input.GetButtonUp("Run")) control.Run(false);

            if (input.GetButtonDown("Execution")) executionManager.DoExecution();
        }

        public void CombatInput()
        {
            if (input.GetButtonDown("Attack")) control.Attack();
        }

        void Update()
        {
            if (input == null || control == null) return;
            // DevInput();
            MoveInput();
            CombatInput();
        }

        public void GetItemSuccess()
        {
            gameActions[0](this);
        }

        public void GetCaught()
        {
            gameActions[1](this);
        }

        public void Goal()
        {
            gameActions[2](this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "PlayerWeapon":
                    Vector2 force = (transform.position - other.transform.parent.position);
                    control.Hurt(new Vector2(force.x, 1) * 12);
                    break;
                case "Flag":
                    Goal();
                    break;
                case "DashItem":
                    string[] handler = other.name.Split(',');
                    float forceX = 0;
                    float forceY = 0;
                    forceX = (string.Compare("n", handler[0]) == 0) ? transform.localScale.x : float.Parse(handler[0]);
                    forceY = (string.Compare("n", handler[1]) == 0) ? transform.localScale.y : float.Parse(handler[1]);
                    control.DODash(new Vector2(
                        forceX,
                        forceY
                    ));
                    break;
                case "RoomTeleport":
                    if (changeLevel == null || isTeleporting) return;
                    isTeleporting = true;
                    switch (other.name)
                    {
                        case "NextCollider":
                            changeLevel[0](this, confiner);
                            break;
                        case "PrevCollider":
                            changeLevel[1](this, confiner);
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
                switch (other.name)
                {
                    case "IceSkate":
                    case "SlimeShoe":
                    case "Shield":
                    case "EnergyDrink":
                    case "Crucifixion":
                    case "Armor":
                    case "LightnessShoe":
                    case "RocketShoe":
                    case "DeveloperObsession":
                    case "Immortal":
                    case "Balloon":
                    case "Trophy":
                    case "Detector":
                        if (input.GetButtonDown("Item"))
                        {
                            Debug.Log("Get item");
                            control.ItemReceived(other.gameObject);
                            control.GetStartItem(other.name, GetItemSuccess);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void Awake()
        {
            control = GetComponent<Control>();
            executionManager = GetComponent<ExecutionManager>();
            if (testMode)
            {
                AssignController(0);
            }
        }
    }
}