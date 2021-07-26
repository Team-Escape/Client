using System;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class HealthPotion : GameItemControl
    {
        public override void Use()
        {
            if (model.health >= model.maxHealth)
                return;
            model.health++;
        }
    }
}