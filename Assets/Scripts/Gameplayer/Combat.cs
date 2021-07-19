using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Combat : GameplayerComponent
    {
        public Combat(View view, Model model)
        {
            this.view = view;
            this.model = model;
        }


    }
}