using UnityEngine;
using System.Collections;
using Gadget.effecter;

namespace Gadget.utility{
    public class Heal : PotionBase {
        [SerializeField]int healMax;
        [SerializeField]int healperTime;
        [SerializeField]float timeset;
        
        protected override void CallWhenUse(IEffecter getEffect){
            getEffect.UseHeal(healMax,healperTime,timeset);
        }
        public override void CallWhenTrigger(IEffecter effecter){
            effecter.UseHeal(healMax,healperTime,timeset);
        }
        protected override void CallWhenThrow(){

        }
    }
}