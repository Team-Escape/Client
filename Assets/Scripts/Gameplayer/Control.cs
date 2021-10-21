using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PlayerSpace.Gameplayer
{
    public class Control : MonoBehaviour
    {
        public bool IsItemNull() => (itemHandler.isEmpty());
        public bool IsGoaled() => isGoaled;
        public CinemachineConfiner GetConfiner() => model.confiner;
        [SerializeField] GameObject itemSystem;
        IitemHandler itemHandler;
        View view;
        Model model;
        Mover mover;
        Combat combat;
        bool isInputKeyboard = false;
        bool ableToMove = true;
        bool isGoaled = false;

        Camera cam { get { return model.cam; } }

        #region UI render
        /// <summary>
        /// Set faceing position with localScaleX.
        /// </summary>
        public void SetLocalScaleXByMovement(float value)
        {
            
            transform.localScale = new Vector2(
                transform.localScale.x >= 0 ?
                ( value >= 0 ? model.characterSize : model.characterSize * -1) :
                ( value <= 0 ? model.characterSize * -1 : model.characterSize)
            , model.characterSize);
            
        }
        /// <summary>
        /// Active UI to hint player press the button.
        /// </summary>
        /// <param name="isActive"></param>
        public void ActiveHintUI(bool isActive) => view.UpdateHintUI(isActive);
        public void ActiveHintUI(bool isActive, Vector3 pos)
        {
            Vector2 newPos = cam.WorldToScreenPoint(new Vector3(pos.x, pos.y, -10));
            view.UpdateHintUI(isActive, newPos);
        }
        #endregion

        #region GameControl
        public void Goal(System.Action callback)
        {
            combat.Goal();
            callback();
        }
        public void AssignControllerType(bool isKeyboard,int playerID)
        {
            if(view==null)view = GetComponent<View>();
            view.Init(isKeyboard);
            isInputKeyboard = isKeyboard;
            SetCamera(playerID);
        }
        public void AssignTeam(int id)
        {
            model.teamID = id;
            combat.AssignTeam();
        }
        /// <summary>
        /// Get startitem implement
        /// </summary>
        /// <param name="go"></param>
        /// <param name="callback"></param>
        public void GetStartItem(GameObject go, System.Action callback)
        {
            if (model.hasGotStartItem == false)
            {
                model.hasGotStartItem = true;

                string name = go.name;
                go.SetActive(false);
                view.UpdateStartItemUI(go.GetComponent<SpriteRenderer>().sprite);
                switch (name)
                {
                    case "IceSkate":
                        model.iceSkate = true;
                        
                        break;
                    case "SlimeShoe":
                        model.slimeShoe = true;
                        
                        break;
                    case "SwiftnessBoot":
                        model.swiftnessBoot = true;
                        model.itemSpeedGain += 0.1f;
                        model.itemJumpGain += 0.1f;
                        
                        break;
                    case "RocketShoe":
                        model.rocketShoe = true;
                        break;
                    case "Shield":
                        model.shield = true;
                        break;
                    case "EnergyDringk":
                        model.energyDrink = true;
                        break;
                    case "ExtralLife":
                        model.extraLife = true;
                        break;
                    case "Armor":
                        model.armor = true;
                        model.itemSpeedGain -= 0.1f;
                        model.itemJumpGain -= 0.1f;
                        model.maxHealth += 2;
                        model.health += 2;
                        break;
                    case "InspectorChance":
                        model.inspectorsChance = true;
                        break;
                    case "DeathWithStronger":
                        model.deathWithStronger = true;
                        break;
                    case "ExtralScore":
                        model.extralScore = true;
                        break;
                    case "Balloon":
                        model.balloon = true;
                        break;
                }

                callback();
            }
        }
        #endregion

        #region In-Game Item
        public void SetGameItem(ItemData item)
        {
            if (!itemHandler.isEmpty())
                return;

            ActiveHintUI(false);

            itemHandler.SetGameItem(item);//itemdata
            view.UpdateGameItemUI(itemHandler.GetCurrentSprite());
        }
        public void UseGameItem()
        {
            if (itemHandler.isEmpty()) return;
            itemHandler.Use();
            view.UpdateGameItemUI(null);
        }
        public void EffectBy(ItemObj item){
            itemHandler.EffectBy(item);
        }
        #endregion

        #region Combat
        public void Attack()
        {
            combat.Attack();
        }
        public void Hurt(Vector2 force, System.Action callback)
        {
            if (combat.isHurting) return;
            mover.Inertance(force);
            combat.Hurt(callback);
        }
        #endregion

        #region Mover
        public void Jump(bool isJumping)
        {
            mover.SetJumping(isJumping);
        }
        public void Run(bool isRunning)
        {
            mover.SetRunning(isRunning);
        }
        public void Move(float movement)
        {
            SetLocalScaleXByMovement(movement);
            mover.SetInput(movement * model.reverseInput);
        }
        public void SetLocalScale(){
            int absLocalScale = (int)(transform.localScale.x/Mathf.Abs(transform.localScale.x));
            SetLocalScaleXByMovement(absLocalScale);
        }
        public void DoDash(Vector2 force)
        {
            mover.DoDash(force * model.dashPower);
        }
        #endregion

        #region PlayerState
        public void OnPlayerStateChaned(PlayerState newState)
        {
            switch (newState)
            {
                case PlayerState.Dead:
                    ableToMove = false;
                    mover.DoForceStop();
                    break;
                case PlayerState.Reborn:
                    ableToMove = false;
                    break;
                case PlayerState.Hunter:
                    ableToMove = true;
                    break;
                case PlayerState.Escaper:
                    ableToMove = true;
                    break;
                case PlayerState.Lockblood:
                    ableToMove = true;
                    break;
                case PlayerState.Invincible:
                    ableToMove = true;
                    break;
                case PlayerState.Spectator:
                    ableToMove = true;
                    break;
            }
        }
        public void PlayerStateHandler()
        {
            if (prePlayerState != playerState)
                OnPlayerStateChaned(playerState);

            prePlayerState = playerState;
        }
        #endregion

        #region Unity Native APIs
        private void Awake()
        {
            view = GetComponent<View>();
            model = GetComponent<Model>();
        }
        private void OnEnable()
        {
            mover = new Mover(view, model);
            combat = new Combat(view, model);
            ItemSystem itemsys = itemSystem.GetComponent<ItemSystem>();
            itemsys.SetPlayerModel(model);
            itemHandler = new Slot(itemsys);   
        }
        void DevInput()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                combat.Hurt(null);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                combat.Dead();
            }
        }
        private void Update()
        {
            DevInput();
            PlayerStateHandler();

            if (ableToMove)
                mover.Update();

            combat.Update();
        }
        private void FixedUpdate()
        {
            if (ableToMove)
                mover.FixedUpdate();

            combat.FixedUpdate();
        }
        #endregion

        #region Getters
        PlayerState playerState { get { return model.CurrentPlayerState; } }
        PlayerState prePlayerState = new PlayerState();
        #endregion

        #region privates
        void SetCamera(int playerID)
        {
            LayerMask layer = LayerMask.NameToLayer("P" + (playerID + 1) + "Cam");
            Camera camera = transform.parent.GetComponentInChildren<Camera>();

            // Open the layer of layer + playerId
            camera.cullingMask |= 1 << layer;

            // Change cinemachine camera child object layer
            Transform follow = transform.parent.GetChild(2);
            follow.gameObject.layer = layer;
        }
        #endregion

    }
}