using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Game
{
    public class Control : MonoBehaviour
    {
        View view = null;
        Model model = null;
        Animator animator = null;
        Rigidbody2D rb = null;
        Mover mover = new Mover();
        Mover tempMover = null;
        Combat combat = new Combat();
        AudioPlayer audioPlayer = null;


        bool isAttacking = false;
        bool isHurting = false;

        bool OnDisable
        {
            get
            {
                if (mover == null || combat == null) return true;
                else return false;
            }
        }

        public void ChangePlayerState(PlayerState state) => model.PlayerState = state;

        public void PlaySoundEffect(string name)
        {
            switch (name)
            {
                case "attack":
                    audioPlayer.PlaySF(this.gameObject, model.attackClip);
                    break;
                case "damage":
                    audioPlayer.PlaySF(this.gameObject, model.damgeClip);
                    break;
            }
        }

        public void GameSetup(int id, int currentEscaperCount)
        {
            model.TeamID = id;
            switch (id)
            {
                case 0:
                    model.PlayerState = PlayerState.Escaper;
                    model.MaxHealth = 3;
                    break;
                case 1:
                    model.PlayerState = PlayerState.Hunter;
                    model.MaxHealth = 10;
                    model.CurrentHealth = 10;
                    model.StateSpeedGain = 1.1f + currentEscaperCount * 0.1f;
                    model.StateJumpGain = 0.9f + currentEscaperCount * 0.1f;
                    break;
                default:
                    Debug.Log("Player teamID errors");
                    break;
            }
        }

        public void hunterDebuff(int currentEscaperCount)
        {
            model.StateSpeedGain = 1.1f + currentEscaperCount * 0.1f;
            model.StateJumpGain = 0.9f + currentEscaperCount * 0.1f;
        }
        public void Move(float value)
        {
            if (OnDisable) return;
            transform.localScale = new Vector2(
                transform.localScale.x >= 0 ?
                value >= 0 ? model.CharacterSize : model.CharacterSize * -1 :
                value <= 0 ? model.CharacterSize * -1 : model.CharacterSize
            , model.CharacterSize);
            mover.SetInput = value;
        }
        public void Jump(bool isJumping)
        {
            if (OnDisable) return;
            mover.Jump(isJumping);
        }
        public void Run(bool value)
        {
            if (OnDisable) return;
            mover.Run(value);
        }
        public void Attack()
        {
            if (OnDisable) return;
            if (isAttacking) return;
            isAttacking = true;

            this.AbleToDo(1f, () => isAttacking = false);

            combat.Attack();
        }
        public void Hurt(Vector2 force, System.Action callback)
        {
            if (isHurting)
                return;
            else if (model.PlayerState == PlayerState.Dead)
            {
                callback();
                Mutate();
            }
            else if (OnDisable)
                return;
            else
            {
                isHurting = true;
                this.AbleToDo(1f, () => isHurting = false);

                mover.Inertance(force);
                combat.Hurt(Dead);
            }
        }
        public void Dead()
        {
            if (model.PlayerState == PlayerState.Dead)
                Mutate();
            else if (OnDisable)
                return;
            else
            {
                combat.Dead();
                tempMover = mover;
                mover = null;
                this.AbleToDo(model.RebornDuration, Reborn);
                // View.UpdateDead();
            }
        }
        public void Reborn()
        {
            mover = tempMover;
            combat.Reborn();
        }
        public void Mutate()
        {
            combat.Mutate(
                transform
            );
            view.UpdaetShaderRenderer("GHOST_ON");
        }
        public void GetStartItem(string name, System.Action callback)
        {
            if (model.IsGetStartItem == false)
            {
                model.IsGetStartItem = true;

                switch (name)
                {
                    case "IceSkate":
                        model.IceSkate = true;
                        break;
                    case "SlimeShoe":
                        model.SlimeShoe = true;
                        break;
                    case "Shield":
                        model.Shielding = true;
                        model.Shield = true;
                        break;
                    case "EnergyDrink":
                        model.EnergyDrink = true;
                        break;
                    case "Crucifixion":

                        model.Crucifixion = true;
                        break;
                    case "Armor":
                        model.AddItemSpeedGain = -0.1f;
                        model.AddItemJumpGain = -0.1f;
                        model.MaxHealth += 2;
                        model.CurrentHealth += 2;
                        model.Armor = true;
                        break;
                    case "LightnessShoe":
                        model.LightnessShoe = true;
                        model.AddItemSpeedGain = 0.1f;
                        model.AddItemJumpGain = 0.1f;
                        break;
                    case "RocketShoe":

                        model.RocketShoe = true;
                        break;
                    case "DeveloperObsession":

                        model.DeveloperObsession = true;
                        break;
                    case "Immortal":

                        model.Immortal = true;
                        break;
                    case "Balloon":
                        model.Balloon = true;
                        break;
                    case "Trophy":

                        model.Trophy = true;
                        break;
                    case "Detector":
                        model.Detector = true;
                        break;
                    default:
                        break;
                }
                callback();
            }
        }
        public void ItemReceived(GameObject target)
        {
            if (model.IsGetStartItem == false)
                target.SetActive(false);
        }
        public void CancelItem()
        {
            model.IsGetStartItem = false;
            model.IceSkate = false;
            model.SlimeShoe = false;
            model.Shield = false;
            model.EnergyDrink = false;
            model.Balloon = false;
            model.Armor = false;
            model.LightnessShoe = false;
            model.Crucifixion = false;
            model.RocketShoe = false;
            model.DeveloperObsession = false;
            model.Immortal = false;
            model.Trophy = false;
            model.Detector = false;
            model.Shielding = false;
            if (model.LightnessShoe)
            {
                model.AddItemSpeedGain = -0.1f;
                model.AddItemJumpGain = -0.1f;
            }
            if (model.Armor)
            {
                model.AddItemSpeedGain = 0.1f;
                model.AddItemJumpGain = 0.1f;
                model.MaxHealth += 2;
            }
        }
        public void SizeAdjust(float size)
        {
            model.CharacterSize *= size;
            //model.CharacterSize = size;

        }
        public void DODash(Vector2 value)
        {
            Debug.Log(value);
            mover.DOAddforceImpulse(value * model.DashPower);
            Debug.Log(model.DashPower);
        }

        private void FixedUpdate()
        {
            if (OnDisable) return;
            mover.FixedUpdate();
        }
        private void Update()
        {
            if (OnDisable)
            {
                view.UpdateHealthBar(0);
                return;
            }
            view.UpdateHealthBar(combat.GetHealthAmount);
            view.UpdateEnduranceBar(mover.GetEnduranceAmount);
            mover.Update();
            //transform.localScale = new Vector3(model.CharacterSize, model.CharacterSize, 1);
        }

        private void Awake()
        {
            view = GetComponent<View>();
            model = GetComponent<Model>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioPlayer = GetComponent<AudioPlayer>();

            // Class Move in old version
            mover = new Mover(this, model, rb, animator);
            combat = new Combat(this, model, animator);

            model.MaxHealth = 3;
            model.Endurance = 3;
        }
    }
}