using UnityEngine;
using System.Collections;

namespace PlayerSpace.Gameplayer
{
    public class Heal : ItemBase
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
                if (model.health < model.maxHealth)
                {
                    if (healMax - (currHeal + healperTime) < 0)
                    {
                        model.health += healMax - currHeal;
                    }
                    else
                    {
                        model.health += healperTime;
                    }
                }
                yield return new WaitForSeconds(delay);

            }
            isenable = false;
        }
    }
}