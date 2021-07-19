using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Mover : GameplayerComponent
    {
        float xInput = 0;
        float jumpTimeCounter = 0f;
        float wallJumpTimeCounter = 0f;
        float wallJumpPos = 0;

        bool ableToJump = true;
        bool isJumping = false;
        bool isWallJumping = false;
        bool isFalling = false;
        bool isJumpButtonReleased = true;
        bool isDoubleJumping = false;

        public float SetInput
        {
            set
            {
                xInput = value;
                anim.DoAnimation("movement", Mathf.Abs(xInput));
            }
        }

        public bool SetJumping
        {
            set
            {
                if (value == true)
                {
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
                }
                else if (value == false)
                {
                    isJumpButtonReleased = true;
                    isFalling = true;
                    isJumping = false;
                    isWallJumping = false;
                    wallJumpTimeCounter = 0;
                    anim.DoAnimation("exit");
                }
            }
        }

        #region GroundChecker
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
                return Physics2D.Raycast(model.transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsGround)
                || Physics2D.Raycast(model.transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsBox);
            }
        }
        public bool OnIceGrounded
        {
            get
            {
                return Physics2D.Raycast(model.transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsIceGround);
            }
        }
        public bool OnSlimeGrounded
        {
            get
            {
                return Physics2D.Raycast(model.transform.position, -Vector2.up, model.distToGround + model.distToGroundOffset, model.whatIsSlimeGround);
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
                return Physics2D.Raycast(model.transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsGround)
                || Physics2D.Raycast(model.transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsBox);
            }
        }
        public bool OnIceFronted
        {
            get
            {
                return Physics2D.Raycast(model.transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsIceGround);
            }
        }
        public bool OnSlimeFronted
        {
            get
            {
                return Physics2D.Raycast(model.transform.position, Vector2.right * xInput, model.distToGround + model.distToWall, model.whatIsSlimeGround);
            }
        }
        #endregion

        public void Update()
        {
            GroundCheck();
            FrontCheck();
        }

        public void FixedUpdate()
        {
            DoMove();
            DoJump();
        }

        public void GroundCheck()
        {
            if (OnGrounded)
            {
                CurrentGroundState = GroundState.Normal;
                model.groundSpeedGain = 1f;
                model.groundJumpGain = 1f;
            }
            else if (OnIceGrounded)
            {
                model.groundSpeedGain = 1f;
                model.groundJumpGain = 1f;

                if (model.iceSkate)
                {
                    CurrentGroundState = GroundState.Normal;
                }
                else
                {
                    CurrentGroundState = GroundState.Ice;
                    if (isFalling)
                    {
                        rb.DoAddforceX(model.transform.localScale.x * 100);
                    }
                }
            }
            else if (OnSlimeGrounded)
            {
                if (model.slimeShoe)
                {
                    CurrentGroundState = GroundState.Normal;
                    model.groundSpeedGain = 1f;
                    model.groundJumpGain = 1f;
                }
                else
                {
                    CurrentGroundState = GroundState.Slime;
                    model.groundSpeedGain = 0.5f;
                    model.groundJumpGain = 0.2f;
                }
            }
            else
            {
                CurrentGroundState = GroundState.Air;
            }

            if (OnAnyGrounded)
            {
                jumpTimeCounter = 0;
                isFalling = false;
                isJumping = false;
                ableToJump = true;
                isDoubleJumping = false;
            }
        }

        public void FrontCheck()
        {
            if (OnFronted)
            {
                model.wallSpeedGain = 1f;
                model.wallJumpGain = 1f;
                model.wallSlideGain = 1f;
            }
            else if (OnIceFronted)
            {
                if (model.iceSkate)
                {
                    model.wallSpeedGain = 1f;
                    model.wallJumpGain = 1f;
                    model.wallSlideGain = 1f;
                }
                else
                {
                    model.wallSpeedGain = 1.3f;
                    model.wallJumpGain = 0f;
                    model.wallSlideGain = 10f;
                }
            }
            else if (OnSlimeFronted)
            {
                if (model.slimeShoe)
                {
                    model.wallSpeedGain = 1f;
                    model.wallJumpGain = 1f;
                    model.wallSlideGain = 1f;
                }
                else
                {
                    model.wallSpeedGain = 0.7f;
                    model.wallJumpGain = 0.7f;
                    model.wallSlideGain = 0.1f;
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
                        rb.DoAddforceImpulse(new Vector2(rb.velocity.x, force));
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

        public void WallSliding()
        {
            rb.DoMove(Vector2.down
                * model.itemJumpGain
                * (2 - model.groundJumpGain)
                * model.jumpGain
                * model.wallSlideGain
            );
        }

        public void ControledMoveHandler()
        {

        }

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

        public Mover(View view, Model model)
        {
            this.view = view;
            this.model = model;
        }

        private void Awake()
        {
            if (OnAnyGrounded)
                isFalling = true;
        }
    }
}