using UnityEngine;
using System.Collections;

namespace Gadget.Utility
{
    public class Heal : GadgetEffect
    {
        [SerializeField] int healMax;
        [SerializeField] int healperTime;
        [SerializeField] float timeset;

        protected override void CallWhenUse()
        {
            //getEffect.UseHeal(healMax,healperTime,timeset);
            StartCoroutine(addHeal(healMax, healperTime, timeset));
        }
        IEnumerator addHeal(int healMax, int healperTime, float delay)
        {

            for (int currHeal = 0; currHeal < healMax; currHeal += healperTime)
            {
                if (model.CurrentHealth < model.MaxHealth)
                {
                    if (healMax - (currHeal + healperTime) < 0)
                    {
                        model.CurrentHealth += healMax - currHeal;
                    }
                    else
                    {
                        model.CurrentHealth += healperTime;
                    }
                }
                yield return new WaitForSeconds(delay);

            }
            enabled = false;
        }
    }
}