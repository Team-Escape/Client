using UnityEngine;

using System.Collections;
namespace PlayerSpace.Gameplayer
{

    public class FaceToAss : ItemBase
    {
        [SerializeField] private float during;



        protected override void CallWhenUse()
        {

            StartCoroutine(FaceToAssForSeconds(during));
        }

        IEnumerator FaceToAssForSeconds(float during)
        {
            model.reverseInput=-1;
            //model.speedGain = Mathf.Abs(model.speedGain) * -1;
            yield return new WaitForSeconds(during);
            //model.speedGain = Mathf.Abs(model.speedGain);
            model.reverseInput=1;
            isenable = false;
        }
    }
}