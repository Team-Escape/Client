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

        public void Jump(bool isJumping)
        {
            mover.SetJumping = isJumping;
        }

        public void Move(float movement)
        {
            SetLocalScaleXByMovement = movement;
            mover.SetInput = movement;
        }

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

        private void Awake()
        {
            view = GetComponent<View>();
            model = GetComponent<Model>();
        }

        private void OnEnable()
        {
            mover = new Mover(view, model);
            // combat = new Mover(view, model);
        }

        private void Update()
        {
            mover.Update();
        }

        private void FixedUpdate()
        {
            mover.FixedUpdate();
        }
    }
}


/*
User -> control -> view | model | mover 
*/