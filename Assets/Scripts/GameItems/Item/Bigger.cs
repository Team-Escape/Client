using UnityEngine;
using System.Collections;

namespace PlayerSpace.Gameplayer
{
    public class Bigger : ItemBase
    {
        [SerializeField] private float during;
        [SerializeField] private float biggerScale;
        [SerializeField] private float stay;
        [SerializeField] private float limitScale;

        protected override void CallWhenUse()
        {
            
            StartCoroutine(BiggerForSeconds());
        }
        
        IEnumerator BiggerForSeconds()
        {
            float defultScale = 1;
            int i = 1;
            while (Mathf.Pow((1+limitScale), i) < biggerScale)
            {
                model.characterSize = defultScale * Mathf.Pow((1+limitScale), i);
                i++;
                yield return new WaitForSeconds(during);
            }

            yield return new WaitForSeconds(stay);
            while (i > 0)
            {
                model.characterSize = defultScale * Mathf.Pow((1+limitScale), i);
                i--;
                yield return new WaitForSeconds(during);
            }
            model.characterSize = defultScale;

            isenable = false;
        }

    }
}