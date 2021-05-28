using UnityEngine;
using Gadget.effecter;
namespace Gadget.utility{
    public class Invisible : PotionBase {
        [SerializeField]private float during;
        protected override void CallWhenUse(IEffecter getEffect){
            getEffect.UseInvisible(during);
        }
        public override void CallWhenTrigger(IEffecter effecter){
            effecter.UseInvisible(during);
        }
        protected override void CallWhenThrow(){

        }
    }
}