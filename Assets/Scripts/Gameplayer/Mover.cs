using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Mover : GameplayerComponent
    {
        public float xInput = 0;
        public float jumpTimeCounter = 0f;
        public float wallJumpTimeCounter = 0f;
        public float wallJumpPos = 0;

        public bool ableToJump = true;
        public bool isRunning = false;
        public bool isAbleToRecoveryEndurance = false;
        public bool isJumping = false;
        public bool isWallJumping = false;
        public bool isFalling = false;
        public bool isInertancing = false;
        public bool isJumpButtonReleased = true;
        public bool isDoubleJumping = false;

        float preEndurance = 0;

        GroundState preGroundState = new GroundState();

        #region Const variables. Game balance related.
        const float RunSpeedGain = 0.2f;
        const float RunJumpGain = 0.1f;

        const float NormalGroundSpeedGain = 1f;
        const float NormalGroundJumpGain = 1f;

        const float SlimeGroundSpeedGain = 0.5f;
        const float SlimeGroundJumpGain = 0.2f;

        const float NormalWallSpeedGain = 1f;
        const float NormalWallJumpGain = 1f;
        const float NormalWallSlideGain = 1f;

        const float IceWallSpeedGain = 1.3f;
        const float IceWallJumpGain = 0f;
        const float IceWallSlideGain = 10f;

        const float SlimeWallSpeedGain = 0.7f;
        const float SlimeWallJumpGain = 0.7f;
        const float SlimeWallSlideGain = 1f;
        #endregion

        public Mover(View view, Model model)
        {
            this.view = view;
            this.model = model;
            OnEnable();
        }
        #region Unity APIs Imitate
        public void OnEnable()
        {
            model.endurance = model.maxEndurance;
        }
        public void Update()
        {
            GroundCheck();
            FrontCheck();
            UpdatePreGroundState();
            EnduranceHandler();
        }
        public void FixedUpdate()
        {
            switch (OnControlled)
            {
                case true:
                    ControledMoveHandler();
                    break;
                case false:
                    DoMove();
                    DoJump();
                    break;
            }
        }
        #endregion

        #region Movement Implement
        public void SetInput(float value)
        {
            xInput = value;
            anim.DoAnimation("movement", Mathf.Abs(xInput));
        }
        public void SetRunning(bool value)
        {
            if (value && isRunning == false)
            {
                isRunning = true;
                isAbleToRecoveryEndurance = false;
                model.speedGain += RunSpeedGain;
                model.jumpGain += RunJumpGain;
            }
            else if (value == false && isRunning)
            {
                isRunning = false;
                AbleToDo(1f, () => isAbleToRecoveryEndurance = true);
                model.speedGain -= RunSpeedGain;
                model.jumpGain -= RunJumpGain;
            }
        }
        public void SetJumping(bool value)
        {
            switch (value)
            {
                case true:
                    if (OnAnyFronted && OnIceFronted == false && isWallJumping == false && isJumping == false)
                    {
                        if (isJumpButtonReleased)
                        {
                            isWallJumping = true;
                            anim.DoAnimation("jump");
                        }
                    }
                    else if (ableToJump && isWallJumping == false)
                    {
                        ableToJump = false;
                        isJumping = true;
                        anim.DoAnimation("jump");
                    }
                    else if (isFalling && isDoubleJumping == false && model.rocketShoe)
                    {
                        isDoubleJumping = true;
                        isJumping = true;
                        jumpTimeCounter = 0;
                        anim.DoAnimation("jump");
                    }
                    isJumpButtonReleased = false;
                    break;
                case false:
                    isJumpButtonReleased = true;
                    isFalling = true;
                    isJumping = false;
                    isWallJumping = false;
                    wallJumpTimeCounter = 0;
                    anim.DoAnimation("exit");
                    break;
            }
        }
        public void GrounStateChanged(GroundState newState)
        {
            switch (newState)
            {
                case GroundState.Ice:
                    float force = (rb.velocity.x > 10) ? 10 : 30;
                    rb.DoAddforceX(transform.localScale.x * force);
                    break;
                case GroundState.Slime:
                    rb.DoMove(Vector2.zero);
                    break;
            }
        }

        public void GroundStateContinuous(GroundState newState)
        {
            switch (newState)
            {
                case GroundState.Ice:
                    rb.DoAddforceX(-rb.velocity.x);
                    break;
            }
        }

        public void UpdatePreGroundState()
        {
            if (preGroundState != model.CurrentGroundState)
                GrounStateChanged(model.CurrentGroundState);
            else
                GroundStateContinuous(model.CurrentGroundState);

            preGroundState = model.CurrentGroundState;
        }

        public void GroundCheck()
        {
            if (OnGrounded)
            {
                CurrentGroundState = GroundState.Normal;
                model.groundSpeedGain = NormalGroundSpeedGain;
                model.groundJumpGain = NormalGroundJumpGain;
            }
            else if (OnIceGrounded)
            {
                if (model.iceSkate)
                {
                    CurrentGroundState = GroundState.Normal;
                }
                else
                {
                    CurrentGroundState = GroundState.Ice;
                }
                model.groundSpeedGain = 1f;
                model.groundJumpGain = 1f;
            }
            else if (OnSlimeGrounded)
            {
                if (model.slimeShoe)
                {
                    CurrentGroundState = GroundState.Normal;
                    model.groundSpeedGain = NormalGroundSpeedGain;
                    model.groundJumpGain = NormalGroundJumpGain;
                }
                else
                {
                    CurrentGroundState = GroundState.Slime;
                    model.groundSpeedGain = SlimeGroundSpeedGain;
                    model.groundJumpGain = SlimeGroundJumpGain;
                    if (isFalling)
                    {
                        rb.DoMove(Vector2.zero);
                    }
                }
            }
            else
            {
                CurrentGroundState = GroundState.Air;
                isFalling = true;
            }

            if (OnAnyGrounded)
            {
                jumpTimeCounter = 0;
                isFalling = false;
                isJumping = false;
                ableToJump = true;
                isWallJumping = false;
                isDoubleJumping = false;
            }
        }

        public void FrontCheck()
        {
            if (OnFronted)
            {
                model.wallSpeedGain = NormalWallSpeedGain;
                model.wallJumpGain = NormalWallJumpGain;
                model.wallSlideGain = NormalWallSlideGain;
            }
            else if (OnIceFronted)
            {
                if (model.iceSkate)
                {
                    model.wallSpeedGain = NormalWallSpeedGain;
                    model.wallJumpGain = NormalWallJumpGain;
                    model.wallSlideGain = NormalWallSlideGain;
                }
                else
                {
                    model.wallSpeedGain = IceWallSpeedGain;
                    model.wallJumpGain = IceWallJumpGain;
                    model.wallSlideGain = IceWallSlideGain;
                }
            }
            else if (OnSlimeFronted)
            {
                if (model.slimeShoe)
                {
                    model.wallSpeedGain = NormalWallSpeedGain;
                    model.wallJumpGain = NormalWallJumpGain;
                    model.wallSlideGain = NormalWallSlideGain;
                }
                else
                {
                    model.wallSpeedGain = SlimeWallSpeedGain;
                    model.wallJumpGain = SlimeWallJumpGain;
                    model.wallSlideGain = SlimeWallSlideGain;
                }
            }
            else
            {
                CurrentFrontState = FrontState.Air;
            }

            if (OnAnyFronted)
            {
                if (isJumpButtonReleased)
                    isWallJumping = false;

                wallJumpTimeCounter = 0;
                wallJumpPos = -xInput;
            }
        }
        public void OnEnduranceChanged(float newVal)
        {
            Debug.Log(newVal / model.maxEndurance);
            view.UpdateEndurancebar(newVal / model.maxEndurance);
        }
        /// <summary>
        /// Control Endurance
        /// </summary>
        public void EnduranceHandler()
        {
            if (isRunning && model.endurance > 0)
            {
                model.endurance -= Time.deltaTime;
            }
            if (isRunning == false && isAbleToRecoveryEndurance && model.endurance < model.maxEndurance)
            {
                SetRunning(false);
                if (model.energyDrink) model.endurance += Time.deltaTime;
                model.endurance += Time.deltaTime;
            }

            if (preEndurance != model.endurance)
                OnEnduranceChanged(model.endurance);

            preEndurance = model.endurance;
        }

        /// <summary>
        /// Enforce to move with a force.
        /// </summary>
        /// <param name="force"></param>
        public void Inertance(Vector2 force)
        {
            isInertancing = true;
            CurrentGroundState = GroundState.Controled;
            rb.DoAddforceImpulse(force);
            AbleToDo(0.2f, () => CurrentGroundState = GroundState.Normal);
            AbleToDo(0.2f, () => isInertancing = false);
        }
        /// <summary>
        /// Impluse rigibody with a force.
        /// </summary>
        /// <param name="force"></param>
        public void DoDash(Vector2 force)
        {
            rb.DoAddforceImpulse(force);
        }
        /// <summary>
        /// Force veloity to zero
        /// </summary>
        public void DoForceStop()
        {
            rb.DoMove(Vector2.zero);
        }
        /// <summary>
        /// Jump control handler
        /// </summary>
        public void DoJump()
        {
            if (isWallJumping)
            {
                WallJump();
            }
            else if (isJumping)
            {
                Jump();
            }
        }
        /// <summary>
        /// Implement a jump action.
        /// </summary>
        public void Jump()
        {
            if (jumpTimeCounter < model.jumpTime)
            {
                jumpTimeCounter += Time.deltaTime;
                float force = Vector2.up.y
                    * model.jumpForce
                    * model.itemJumpGain
                    * model.groundJumpGain
                    * model.jumpGain
                    * model.playerStateJumpGain;
                switch (CurrentGroundState)
                {
                    case GroundState.Ice:
                        rb.DoMove(new Vector2(rb.velocity.x, force));
                        break;
                    default:
                        rb.DoMoveY(force);
                        break;
                }
            }
            else
            {
                isJumping = false;
            }
        }
        /// <summary>
        ///  Impletment a wall jumping action.
        /// </summary>
        public void WallJump()
        {
            if (wallJumpTimeCounter < model.wallJumpTime && wallJumpPos * xInput < 0)
            {
                wallJumpTimeCounter += Time.deltaTime;
                rb.DoMove(new Vector2(
                    model.wallJumpForce.x * wallJumpPos
                    * model.itemJumpGain
                    * model.jumpGain
                    * model.playerStateJumpGain
                    * model.wallJumpGain
                    * (2.2f - wallJumpTimeCounter * 10)
                , model.wallJumpForce.y
                    * model.itemJumpGain
                    * model.jumpGain
                    * model.playerStateJumpGain
                    * model.wallJumpGain));
                isJumpButtonReleased = false;
            }
            else
            {
                isWallJumping = false;
            }
        }
        /// <summary>
        /// Move control handler
        /// </summary>
        public void DoMove()
        {
            if (OnAnyFronted)
            {
                WallSliding();
            }
            else
            {
                switch (CurrentGroundState)
                {
                    case GroundState.Controled:
                        ControledMoveHandler();
                        break;
                    case GroundState.Ice:
                        IceMoveHandle();
                        break;
                    case GroundState.Air:
                    case GroundState.Normal:
                    case GroundState.Slime:
                    default:
                        MoveHandler();
                        break;

                }
            }
        }
        /// <summary>
        /// Implemetn a wall sliding action.
        /// </summary>
        public void WallSliding()
        {
            rb.DoMove(Vector2.down
                * model.itemJumpGain
                * (2 - model.groundJumpGain)
                * model.jumpGain
                * model.wallSlideGain
            );
        }
        /// <summary>
        /// Implement move action on controlled state.
        /// </summary>
        public void ControledMoveHandler()
        {
            if (OnAnyFronted)
            {
                DoMove();
            }
            else
            {
                rb.DoAddforce(-rb.velocity * xInput);
            }
        }
        /// <summary>
        /// Usual moving action implement.
        /// </summary>
        public void MoveHandler()
        {
            rb.DoMoveX(xInput
                * model.moveSpeed
                * model.itemSpeedGain
                * model.groundSpeedGain
                * model.speedGain
                * model.playerStateSpeedGain
                * model.slideSpeedGain
            );
        }
        /// <summary>
        /// Moving action implement on ice material .
        /// </summary>
        public void IceMoveHandle()
        {
            rb.DoAddforceX(xInput * 3
                * model.moveSpeed
                * model.itemSpeedGain
                * model.groundSpeedGain
                * model.speedGain
                * model.playerStateSpeedGain
                * model.slideSpeedGain
            );
        }
        #endregion

        #region Getters
        public bool OnControlled
        {
            get
            {
                return rb.velocity.magnitude > 30f;
            }
        }
        public bool OnAnyGrounded
        {
            get
            {
                return OnGrounded || OnIceGrounded || OnSlimeGrounded;
            }
        }
        public bool OnGrounded
        {
            get
            {
                return Physics2D.Raycast(transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsGround)
                || Physics2D.Raycast(transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsBox);
            }
        }
        public bool OnIceGrounded
        {
            get
            {
                return Physics2D.Raycast(transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsIceGround);
            }
        }
        public bool OnSlimeGrounded
        {
            get
            {
                return Physics2D.Raycast(transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsSlimeGround);
            }
        }

        public bool OnAnyFronted
        {
            get
            {
                return OnFronted || OnIceFronted || OnSlimeFronted;
            }
        }
        public bool OnFronted
        {
            get
            {
                return Physics2D.Raycast(transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsGround)
                || Physics2D.Raycast(transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsBox);
            }
        }
        public bool OnIceFronted
        {
            get
            {
                return Physics2D.Raycast(transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsIceGround);
            }
        }
        public bool OnSlimeFronted
        {
            get
            {
                return Physics2D.Raycast(transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsSlimeGround);
            }
        }
        #endregion
    }
}