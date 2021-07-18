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
    }
}


/*
User -> control -> view | model | mover 
*/