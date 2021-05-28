using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public enum PlayerState { Waiting, Escaper, Hunter, Dead, Reborn, Spectator };
    public enum GroundState { Controled, Air, Normal, Ice, Slime };
    public enum FrontState { Controled, Air, Normal, Ice, Slime };
    public enum JumpState { PreWallSliding, IsWallSliding, PreWallJumping, IsWallJumping, PreGrounded, IsGrounded, PreJumping, IsJumping, PreFalling, IsFalling };
    public static class RoleExtension
    {
        public static void AbleToDo<T>(this T source, float sec, System.Action action) where T : MonoBehaviour
        {
            source.StartCoroutine(DelaySec(sec, action));
        }
        public static IEnumerator DelaySec(float sec, System.Action callback)
        {
            yield return new WaitForSeconds(sec);
            callback();
        }
        public static bool StateCompare<T>(this T source, string name) where T : System.Enum
        {
            return ((System.Enum)source == System.Enum.Parse(typeof(T), name, true));
        }
        public static T ToEnum<T>(this string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }
    }
    public static class RoleAnimationExtension
    {
        public static void DoAnimation(this Animator animator, string name)
        {
            animator.SetTrigger(name);
        }
        public static void DoAnimation(this Animator animator, string name, float value)
        {
            animator.SetFloat(name, value);
        }
    }
    public static class RoleMoveExtension
    {
        public static void DoMove(this Rigidbody2D rb, Vector2 force)
        {
            rb.velocity = force;
        }
        public static void DoAddMove(this Rigidbody2D rb, Vector2 force)
        {
            rb.velocity += force;
        }
        public static void DoMoveX(this Rigidbody2D rb, float force)
        {
            rb.velocity = new Vector2(force, rb.velocity.y);
        }
        public static void DoMoveY(this Rigidbody2D rb, float force)
        {
            rb.velocity = new Vector2(rb.velocity.x, force);
        }
        public static void DoControlMove(this Rigidbody2D rb, float force)
        {
            rb.DoAddforce(new Vector2(rb.velocity.x, rb.velocity.y));
        }
        public static void DoAddforce(this Rigidbody2D rb, Vector2 force)
        {
            rb.AddForce(force);
        }
        public static void DoAddforceImpulse(this Rigidbody2D rb, Vector2 force)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        public static void DoAddforceX(this Rigidbody2D rb, float force)
        {
            rb.AddForce(new Vector2(force, 0));
        }
        public static void DoAddforceY(this Rigidbody2D rb, float force)
        {
            rb.AddForce(new Vector2(0, force));
        }
        public static void DoStopMoveX(this Rigidbody2D rb)
        {
            rb.DoMoveX(0);
        }
        public static void DoSlowDown(this Rigidbody2D rb)
        {
            rb.DoAddMove(rb.velocity * -0.05f);
        }
        public static void DlIceSlide(this Rigidbody2D rb, float posNForce)
        {
            rb.DoAddforceImpulse(Vector2.right * posNForce);
        }
        public static void DoSlide(this Rigidbody2D rb)
        {
            rb.DoAddMove(rb.velocity * -0.1f);
        }
        public static void DoDash(this Rigidbody2D rb, float force)
        {

        }
    }
}