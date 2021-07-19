using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Combat : GameplayerComponent
    {
        #region PlayerState Quick Search
        PlayerState AsWaiting { get { return PlayerState.Waiting; } }
        PlayerState AsEscaper { get { return PlayerState.Escaper; } }
        PlayerState AsHunter { get { return PlayerState.Hunter; } }
        PlayerState AsDead { get { return PlayerState.Dead; } }
        PlayerState AsInvincible { get { return PlayerState.Invincible; } }
        PlayerState AsReborn { get { return PlayerState.Reborn; } }
        PlayerState AsSpectator { get { return PlayerState.Spectator; } }
        PlayerState AsLockBlood { get { return PlayerState.Lockblood; } }
        #endregion

        public bool isAttacking = false;
        public bool isHurting = false;
        public bool isShielding = false;

        public Combat(View view, Model model)
        {
            this.view = view;
            this.model = model;
            ActiveStartItemIfEquip();
        }

        public void ActiveStartItemIfEquip()
        {
            if (model.shield)
            {
                isShielding = true;
            }
        }

        public void Attack()
        {
            if (isAttacking) return;
            if (CurrentPlayerState == AsHunter)
            {
                isAttacking = true;
                anim.DoAnimation("attack");
                AbleToDo(1f, () => isAttacking = false);
            }
        }

        public void Hurt()
        {
            if (isHurting) return;
            if (isShielding)
            {
                isShielding = false;
                AbleToDo(model.sheildColdDuration, () => isShielding = true);
                return;
            }
            switch (CurrentPlayerState)
            {
                case PlayerState.Lockblood:
                    if (model.health > 1)
                        model.health--;
                    break;
                default:
                    model.health--;
                    if (model.health <= 0)
                    {
                        model.health = 0;
                        Dead();
                        return;
                    }
                    break;
            }
            isHurting = true;
            anim.DoAnimation("hurt");
            AbleToDo(1f, () => isHurting = false);
        }

        public void Dead()
        {
            CurrentPlayerState = AsDead;
            anim.DoAnimation("dead");
            AbleToDo(model.rebornDuration, Reborn);
        }

        public void Reborn()
        {
            if (CurrentPlayerState != AsDead) return;

            CurrentPlayerState = AsReborn;
            anim.DoAnimation("reborn");
            AbleToDo(anim.CurrentAnimationClipLength(),
                () => CurrentPlayerState = (model.teamID == 1)
                ? AsHunter
                : AsEscaper
            );
        }

        public void Mutate()
        {
            CurrentPlayerState = AsSpectator;

            int playerLayer = LayerMask.NameToLayer("Player");
            int sepecatorLayer = LayerMask.NameToLayer("Invisible");

            Camera cam = model.cam;
            cam.cullingMask = -1;

            anim.DoAnimation("reborn");
            view.UpdateShaderRender("GHOST_ON");

            SearchForAllChild(transform, playerLayer, sepecatorLayer);
        }

        public void SearchForAllChild(Transform t, int layerToBeChanged, int layerToChange)
        {
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