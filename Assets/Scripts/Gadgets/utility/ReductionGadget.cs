using UnityEngine;
using Gadget.Effector;
using ObjectPool;
using System.Collections;
using PlayerSpace.Game;
namespace Gadget.Utility
{
    public class ReductionGadget : GadgetEffect
    {
        [SerializeField] private float during;
        [SerializeField] private float smallerScale;
        [SerializeField] private float stay;
        [SerializeField] private float limitScale;

        protected override void CallWhenUse()
        {
            CallWhenTrigger();
        }
        public void CallWhenTrigger()
        {
            //effecter.UseReduction(during,smallerScale,stay, limitScale);

            StartCoroutine(ReductionForSeconds(during, smallerScale, stay, limitScale));
        }


        IEnumerator ReductionForSeconds(float during, float smallerScale, float stay, float limitScale)
        {
            float defultScale = model.CharacterSize;
            int i = 1;
            while (Mathf.Pow(limitScale, i) > smallerScale)
            {
                model.CharacterSize = defultScale * Mathf.Pow(limitScale, i);
                i++;
                yield return new WaitForSeconds(during);
            }

            yield return new WaitForSeconds(stay);
            while (i > 0)
            {
                model.CharacterSize = defultScale * Mathf.Pow(limitScale, i);
                i--;
                yield return new WaitForSeconds(during);
            }
            model.CharacterSize = defultScale;

            enabled = false;
        }

    }
}