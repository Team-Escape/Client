using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Game
{
    public class Combat
    {
        MonoBehaviour mono = null;
        Model model = null;
        Animator animator = null;

        public PlayerState PlayerState { get { return model.PlayerState; } set { model.PlayerState = value; } }
        public PlayerState AsWaiting { get { return PlayerState.Waiting; } }
        public PlayerState AsEscaper { get { return PlayerState.Escaper; } }
        public PlayerState AsHunter { get { return PlayerState.Hunter; } }
        public PlayerState AsSpectator { get { return PlayerState.Spectator; } }

        public float GetHealthAmount
        {
            get
            {
                return (float)model.CurrentHealth / (float)model.MaxHealth;
            }
        }


        public Combat() { }
        public Combat(MonoBehaviour _mono, Model _model, Animator _animator)
        {
            mono = _mono;
            model = _model;
            animator = _animator;
            model.CurrentHealth = model.MaxHealth;
        }

        public void Attack()
        {
            if (PlayerState == PlayerState.Hunter)
                animator.DoAnimation("attack");
        }
        public void Hurt(System.Action callback)
        {
            if (PlayerState == PlayerState.Spectator) return;
            if (model.Shielding == false)
            {
                model.CurrentHealth--;
                if (model.CurrentHealth <= 0)
                {
                    model.CurrentHealth = 0;
                    callback();
                    return;
                }
                animator.DoAnimation("hurt");
            }
            else model.Shielding = false;
        }
        public void Dead()
        {
            PlayerState = PlayerState.Dead;
            animator.DoAnimation("dead");
        }
        public void Reborn()
        {
            if (PlayerState != PlayerState.Dead) return;
            PlayerState = PlayerState.Reborn;
            mono.AbleToDo(0.5f,
                () => PlayerState = model.TeamID == 1
                ? PlayerState.Escaper
                : PlayerState.Hunter
            );
            model.CurrentHealth = model.MaxHealth;
            animator.DoAnimation("reborn");
        }
        public void Mutate(Transform transform)
        {
            PlayerState = PlayerState.Spectator;

            int playerLayer = 24;
            int specatorLayer = 23;

            // Open all layer on culling mask.
            Camera cam = transform.parent.GetChild(1).GetComponent<Camera>();
            cam.cullingMask = -1;

            // Set all layers to invisible
            SearchForAllChild(transform.parent, playerLayer, specatorLayer);
        }

        void SearchForAllChild(Transform t, int layerToBeChanged, int layerToChange)
        {
            if (t == null) return;
            foreach (Transform c in t)
            {
                SearchForAllChild(c, layerToBeChanged, layerToChange);
                if (c.gameObject.layer == layerToBeChanged)
                {
                    c.gameObject.layer = layerToChange;
                }
            }
        }
    }
}