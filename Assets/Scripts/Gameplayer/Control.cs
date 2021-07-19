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

        public void AssignTeam(int id)
        {
            model.teamID = id;
        }

        public void Hurt(Vector2 force)
        {
            if (combat.isHurting) return;
            mover.Inertance(force);
            combat.Hurt();
        }

        public void Jump(bool isJumping)
        {
            mover.SetJumping = isJumping;
        }

        public void Move(float movement)
        {
            SetLocalScaleXByMovement = movement * model.reverseInput;
            mover.SetInput = movement * model.reverseInput;
        }

        public void DoDash(Vector2 force)
        {
            mover.DoDash(force * model.dashPower);
        }

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

        private void Update()
        {
            mover.Update();
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

        private void FixedUpdate()
        {
            mover.FixedUpdate();
        }
    }
}