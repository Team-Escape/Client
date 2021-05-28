using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Game
{
    public class Mover
    {
        MonoBehaviour mono = null;
        Model model = null;
        Rigidbody2D rb = null;
        Animator animator = null;
        /* -------------------------------------------------------------------------- */
        /*                                 enum                                       */
        /* -------------------------------------------------------------------------- */
        GroundState GroundState { get { return model.GroundState; } set { model.GroundState = value; } }
        FrontState FrontState { get { return model.FrontState; } set { model.FrontState = value; } }
        JumpState JumpState { get { return model.JumpState; } set { model.JumpState = value; } }
        /* -------------------------------------------------------------------------- */
        /*                                 float                                      */
        /* -------------------------------------------------------------------------- */
        public float SetInput
        {
            set
            {
                horizontalInput = value;
                animator.DoAnimation("movement", Mathf.Abs(horizontalInput));
            }
        }
        float horizontalInput = 0;
        public float GetEnduranceAmount { get { return endurance / model.Endurance; } }
        float endurance = 0;
        float jumpTimeCounter = 0;
        float wallJumpTimeCounter = 0;
        float wallJumpPos = 0;
        /* -------------------------------------------------------------------------- */
        /*                                 bool                                       */
        /* -------------------------------------------------------------------------- */
        bool isRunning = false;
        bool isAbleToRecoveryEndurance = false;
        bool isSquating = false;
        bool isWallJumping = false;
        bool isInertancing = false;
        public bool OnControl
        {
            get
            {
                if (rb.velocity.magnitude > 40 || isInertancing)
                    return true;
                return false;
            }
        }
        public bool OnGround
        {
            get
            {
                return model.Grounded || model.IceGrounded || model.SlimeGrounded;
            }
        }
        public bool OnWall
        {
            get
            {
                if (OnGround)
                    return false;
                else
                    return model.Fronted || model.IceFronted || model.SlimeFronted;
            }
        }

        /* -------------------------------------------------------------------------- */
        /*                              Constructor                                   */
        /* -------------------------------------------------------------------------- */
        public Mover() { }
        public Mover(MonoBehaviour _mono, Model _model, Rigidbody2D _rb, Animator _animator)
        {
            mono = _mono;
            model = _model;
            rb = _rb;
            animator = _animator;

            JumpState = JumpState.PreFalling;
            FrontState = FrontState.Air;
            GroundState = GroundState.Air;

            endurance = model.Endurance;
        }

        public void Update()
        {
            EnduranceSystem();
            GroundControl();
            GroundControl();
            FrontControl();
        }
        public void FixedUpdate()
        {
            DoMove();
            DoJump();
        }
        /* -------------------------------------------------------------------------- */
        /*                                 APIs                                       */
        /* -------------------------------------------------------------------------- */
        public void Inertance(Vector2 force)
        {
            isInertancing = true;
            GroundState = GroundState.Controled;
            rb.AddForce(force, ForceMode2D.Impulse);
            mono.AbleToDo(0.2f, () => GroundState = GroundState.Normal);
            mono.AbleToDo(0.2f, () => isInertancing = false);
        }
        public void DoMove()
        {
            switch (GroundState)
            {
                case GroundState.Controled:
                    rb.DoControlMove(horizontalInput);
                    break;
                case GroundState.Air:
                case GroundState.Normal:
                case GroundState.Slime:
                    if (horizontalInput != 0)
                    {
                        MoveGainHandle();
                    }
                    else rb.DoStopMoveX();
                    break;
                case GroundState.Ice:

                    if (horizontalInput != 0)
                    {
                        IceMoveGainHandle();
                        rb.DoSlowDown();
                    }
                    else rb.DoSlowDown();
                    break;
                default:
                    break;
            }
        }
        public void Run(bool value)
        {
            switch (value)
            {
                case true:
                    isRunning = true;
                    isAbleToRecoveryEndurance = false;
                    model.AddSpeedGain = 0.2f;
                    model.AddJumpGain = 0.1f;
                    break;
                case false:
                    isRunning = false;
                    mono.AbleToDo(1f, () => isAbleToRecoveryEndurance = true);
                    model.SpeedGain = 1f;
                    model.JumpGain = 1f;
                    break;
            }
        }
        public void EnduranceSystem()
        {
            if (endurance <= 0)
                Run(false);
            if (isRunning)
                endurance -= Time.deltaTime;
            if (isRunning == false && isAbleToRecoveryEndurance && endurance < model.Endurance)
            {
                if (model.EnergyDrink) endurance += Time.deltaTime;
                endurance += Time.deltaTime;
            }
        }
        public void MoveGainHandle()
        {
            if (isSquating && rb.velocity.x == 0) model.SlideSpeedGain = 0;
            else if (isSquating && model.SlideSpeedGain > 0) model.SlideSpeedGain -= 0.015f;
            else if (isSquating && model.SlideSpeedGain <= 0) model.SlideSpeedGain = 0;
            else model.SlideSpeedGain = 1f;
            FrontControl();
            if (OnWall)
            {
                rb.DoMoveX(0);
            }
            else
            {
                rb.DoMoveX(horizontalInput
                    * model.WalkSpeed
                    * model.ItemSpeedGain
                    * model.GroundSpeedGain
                    * model.SpeedGain
                    * model.StateSpeedGain
                    * model.SlideSpeedGain);
            }

        }
        public void IceMoveGainHandle()
        {
            FrontControl();
            if (rb.velocity.x < 15)
            {
                if (OnWall)
                {
                    rb.DoAddforceX(0);
                }
                else
                {
                    rb.DoAddforceX(horizontalInput * 3
                        * model.WalkSpeed
                        * model.ItemSpeedGain
                        * model.GroundSpeedGain
                        * model.SpeedGain
                        * model.StateSpeedGain
                        * model.SlideSpeedGain);
                }
            }
        }
        public void Jump(bool isJumping)
        {
            if (isJumping)
            {
                FrontControl();
                GroundControl();
                if (OnGround)
                {
                    JumpState = JumpState.PreJumping;
                }
                else if (OnWall)
                {
                    isWallJumping = true;
                    JumpState = JumpState.PreWallJumping;
                }
                else if (!OnGround && !OnWall && model.RocketShoe && !model.RocketJump)
                {
                    model.RocketJump = true;
                    jumpTimeCounter = 0f;
                    model.GroundJumpGain = 0.5f;
                    JumpState = JumpState.PreJumping;
                }
                animator.DoAnimation("jump");
            }
            else
            {
                if (JumpState == JumpState.IsJumping)
                {
                    JumpState = JumpState.PreFalling;
                }
            }
        }
        public void DoJump()
        {
            switch (JumpState)
            {
                case JumpState.PreWallSliding:
                    JumpState = JumpState.IsWallSliding;
                    break;
                case JumpState.PreWallJumping:
                    JumpState = JumpState.IsWallJumping;
                    break;
                case JumpState.PreJumping:
                    JumpState = JumpState.IsJumping;
                    break;
                case JumpState.PreFalling:
                    JumpState = JumpState.IsFalling;
                    break;
                case JumpState.PreGrounded:
                    animator.DoAnimation("exit");
                    JumpState = JumpState.IsGrounded;
                    break;
            }
            switch (JumpState)
            {
                case JumpState.IsWallSliding:
                    model.RocketJump = false;
                    //if (wallJumpTimeCounter > 0) wallJumpTimeCounter = 0;
                    rb.DoMove(Vector2.down
                        * model.ItemJumpGain
                        * (2 - model.GroundJumpGain)
                        * model.JumpGain
                        * model.WallSlideGain);
                    if (OnWall == false || OnGround) JumpState = JumpState.PreFalling;
                    break;
                case JumpState.IsWallJumping:
                    if (wallJumpTimeCounter < model.WallJumpTime)
                    {
                        wallJumpTimeCounter += Time.deltaTime;
                        rb.DoMove(new Vector2(
                            model.WallJumpForce.x * wallJumpPos
                            * model.ItemJumpGain
                            * model.JumpGain
                            * model.StateJumpGain
                            * model.WallJumpGain
                        , model.WallJumpForce.y
                            * model.ItemJumpGain
                            * model.JumpGain
                            * model.StateJumpGain
                            * model.WallJumpGain));
                    }
                    else
                    {
                        isWallJumping = false;
                        JumpState = JumpState.PreFalling;
                    }
                    break;
                case JumpState.IsJumping:
                    if (jumpTimeCounter < model.JumpTime)
                    {
                        jumpTimeCounter += Time.deltaTime;
                        rb.DoMoveY(Vector2.up.y * model.JumpForce
                        * model.ItemJumpGain
                        * model.GroundJumpGain
                        * model.JumpGain
                        * model.StateJumpGain);
                    }
                    else JumpState = JumpState.PreFalling;
                    break;
                case JumpState.IsFalling:
                    if (OnGround) JumpState = JumpState.PreGrounded;
                    break;
                case JumpState.IsGrounded:
                    jumpTimeCounter = 0;
                    model.RocketJump = false;
                    break;
            }
        }
        public void GroundControl()
        {
            // if (GroundState == GroundState.controled) return;
            model.Grounded = GroundCheck(model.GetGroundLayer) || GroundCheck(model.GetBoxLayer);
            model.IceGrounded = GroundCheck(model.GetIceGroundLayer);
            model.SlimeGrounded = GroundCheck(model.GetSlimeGroundLayer);

            if (OnControl)
            {
                GroundState = GroundState.Controled;
            }
            else if (model.Grounded)
            {
                model.GroundSpeedGain = 1;
                model.GroundJumpGain = 1f;
                model.GroundState = GroundState.Normal;
            }
            else if (model.IceGrounded)
            {
                model.GroundSpeedGain = 1;
                model.GroundJumpGain = 1;
                if (model.IceSkate == false) model.GroundState = GroundState.Ice;
                else model.GroundState = GroundState.Normal;
            }
            else if (model.SlimeGrounded)
            {
                if (model.SlimeShoe == false)
                {
                    model.GroundSpeedGain = 0.5f;
                    model.GroundJumpGain = 0.2f;
                    model.GroundState = GroundState.Slime;
                }
                else model.GroundState = GroundState.Normal;
            }
            else
            {
                model.GroundState = GroundState.Air;
            }
        }
        public void FrontControl()
        {
            if (OnWall) if (wallJumpTimeCounter > 0) wallJumpTimeCounter = 0;
            if (OnGround)
            {
                return;
            }
            // if (FrontState == FrontState.controled) return;

            model.Fronted = FrontCheck(model.GetGroundLayer);
            model.IceFronted = FrontCheck(model.GetIceGroundLayer);
            model.SlimeFronted = FrontCheck(model.GetSlimeGroundLayer);

            if (model.Fronted)
            {
                model.WallSpeedGain = 1f;
                model.WallJumpGain = 1f;
                model.WallSlideGain = 1f;
            }
            else if (model.IceFronted && OnWall)
            {
                if (model.IceSkate == false)
                {
                    model.WallSpeedGain = 1.3f;
                    model.WallJumpGain = 0f;
                    model.WallSlideGain = 10f;
                }
                else
                {
                    model.WallSpeedGain = 1f;
                    model.WallJumpGain = 1f;
                    model.WallSlideGain = 1f;
                }
            }
            else if (model.SlimeFronted && OnWall)
            {
                if (model.SlimeShoe == false)
                {
                    model.WallSpeedGain = 0.7f;
                    model.WallJumpGain = 0.7f;
                    model.WallSlideGain = 1f;
                }
                else
                {
                    model.WallSpeedGain = 1f;
                    model.WallJumpGain = 1f;
                    model.WallSlideGain = 1f;
                }
            }

            if (OnGround == false && isWallJumping == false && JumpState != JumpState.IsJumping && JumpState != JumpState.PreJumping) JumpState = JumpState.PreFalling;
            if (isWallJumping == false && OnWall && rb.velocity.y < 0 && JumpState != JumpState.PreWallJumping) JumpState = JumpState.PreWallSliding;
        }
        public bool GroundCheck(LayerMask mask)
        {
            bool detect = false;
            foreach (var ground in model.GetGroundCheck)
            {
                if (Physics2D.Raycast(
                    ground.position,
                    Vector2.down,
                    model.GroundCheckDistance,
                    mask
                ))
                {
                    detect = true;
                    break;
                }
            };
            return detect ? true : false;
        }
        public bool FrontCheck(LayerMask mask)
        {
            bool detect = false;
            foreach (var front in model.GetFrontCheck)
            {
                if (Physics2D.Raycast(
                    front.position,
                    Vector2.right * horizontalInput,
                    model.GroundCheckDistance,
                    mask
                ))
                {
                    detect = true;
                    break;
                }
            };
            if (detect) wallJumpPos = -horizontalInput;
            return detect ? true : false;
        }
        public void DOAddforceImpulse(Vector2 power)
        {
            rb.DoAddforceImpulse(power);
        }
    }
}