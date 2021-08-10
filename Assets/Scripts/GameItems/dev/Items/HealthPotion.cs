using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    
    public class HealthPotion : GameItemControl_Mono
    {
        [SerializeField] int addHealth = 0;
        public override void Use()
        {
            if (model.health >= model.maxHealth)
                return;
            model.health+=addHealth;
            Recycle();
        }
    }
}