﻿using System.Linq;
using System.Collections;
using UnityEngine;

namespace PlayerSpace
{
    [System.Serializable]
    public enum PlayerState { Waiting, Escaper, Hunter, Dead, Reborn, Spectator, Lockblood, Invincible };
    public enum GroundState { Controled, Air, Normal, Ice, Slime };
    public enum FrontState { Controled, Air, Normal, Ice, Slime };
    public enum JumpState { PreWallSliding, IsWallSliding, PreWallJumping, IsWallJumping, PreGrounded, IsGrounded, PreJumping, IsJumping, PreFalling, IsFalling };
    public static class RoleExtension
    {
        // public static void SetValueInTime<T>(this T source, float sec, ref float currentVal, float endVal) where T : MonoBehaviour
        // {
        //     source.StartCoroutine(AddValbyTime(sec, currentVal, endVal));
        // }
        // public static IEnumerator AddValbyTime(float sec, float currentVal, float endVal)
        // {
        //     while (Mathf.Abs(startVal - endVal) > 0.01f)
        //     {
        //         startVal += Time.deltaTime;
        //         yield return new WaitForSecondsRealtime(Time.deltaTime / sec);
        //     }
        // }
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
        public static float CurrentAnimationClipLength(this Animator animator, string clipName)
        {
            Debug.Log(animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == clipName));
            return animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == clipName).length;
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
            rb.DoAddforce(new Vector2(force, rb.velocity.y));
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
    }
}