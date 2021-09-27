using UnityEngine;
using System.Collections;

namespace PlayerSpace.Gameplayer
{
    public class ReductionItem : ItemBase
    {
        [SerializeField] private float during;
        [SerializeField] private float smallerScale;
        [SerializeField] private float stay;
        [SerializeField] private float limitScale;

        protected override void CallWhenUse()
        {
            
            StartCoroutine(ReductionForSeconds(during, smallerScale, stay, limitScale));
        }
        
        IEnumerator ReductionForSeconds(float during, float smallerScale, float stay, float limitScale)
        {
            float defultScale = 1;
            int i = 1;
            while (Mathf.Pow(limitScale, i) > smallerScale)
            {
                model.characterSize = defultScale * Mathf.Pow(limitScale, i);
                i++;
                yield return new WaitForSeconds(during);
            }

            yield return new WaitForSeconds(stay);
            while (i > 0)
            {
                model.characterSize = defultScale * Mathf.Pow(limitScale, i);
                i--;
                yield return new WaitForSeconds(during);
            }
            model.characterSize = defultScale;

            isenable = false;
        }

    }
}