using System;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    [CreateAssetMenu (menuName="item/HealthPotion")]
    public class HealthPotion : GameItemControl
    {
        [SerializeField] int addHealth = 0;
        public override void Use()
        {
            if (model.health >= model.maxHealth)
                return;
            model.health+=addHealth;
        }
    }
}