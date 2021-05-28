using UnityEngine;
using Gadget.effecter;
namespace Gadget.utility{
    public class ReductionGadget : PotionBase {
        [SerializeField]private float during;
        [SerializeField]private float smallerScale;
        [SerializeField]private float stay;
        [SerializeField]private float limitScale;
        protected override void CallWhenUse(IEffecter getEffect){
            getEffect.UseReduction(during,smallerScale,stay, limitScale);
        }
        public override void CallWhenTrigger(IEffecter effecter){
            effecter.UseReduction(during,smallerScale,stay, limitScale);
        }
        protected override void CallWhenThrow(){

        }
    }
}