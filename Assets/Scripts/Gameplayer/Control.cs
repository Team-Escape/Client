using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Control : MonoBehaviour
    {
        View view;
        Model model;
        Mover mover;
        Combat combat;

        bool isInputKeyboard = false;
        bool ableToMove = true;

        #region UI render
        /// <summary>
        /// Set faceing position with localScaleX.
        /// </summary>
        public float SetLocalScaleXByMovement
        {
            set
            {
                transform.localScale = new Vector2(
                    transform.localScale.x >= 0 ?
                    value >= 0 ? model.characterSize : model.characterSize * -1 :
                    value <= 0 ? model.characterSize * -1 : model.characterSize
                , model.characterSize);
            }
        }
        /// <summary>
        /// Active UI to hint player press the button.
        /// </summary>
        /// <param name="isActive"></param>
        public void ActiveHintUI(bool isActive) => view.UpdateHintUI(isActive);
        #endregion

        #region GameControl
        public void AssignControllerType(bool isKeyboard)
        {
            view.Init(isKeyboard);
            isInputKeyboard = isKeyboard;
        }

        public void AssignTeam(int id)
        {
            model.teamID = id;
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
                    case "Balloon":
                        model.balloon = true;
                        break;
                }

                callback();
            }
        }
        #endregion

        #region Combat
        public void Attack()
        {
            combat.Attack();
        }

        public void Hurt(Vector2 force)
        {
            if (combat.isHurting) return;
            mover.Inertance(force);
            combat.Hurt();
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
            SetLocalScaleXByMovement = movement * model.reverseInput;
            mover.SetInput(movement * model.reverseInput);
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
        }

        void DevInput()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                combat.Hurt();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                combat.Dead();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                combat.Mutate();
            }
        }


        private void Update()
        {
            DevInput();
            PlayerStateHandler();

            if (ableToMove)
                mover.Update();

        }

        private void FixedUpdate()
        {
            if (ableToMove)
                mover.FixedUpdate();
        }
        #endregion

        #region Getters
        PlayerState playerState { get { return model.CurrentPlayerState; } }
        PlayerState prePlayerState = new PlayerState();
        #endregion
    }
}