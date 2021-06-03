using UnityEngine;

using System.Collections;
namespace Gadget.Utility
{

    public class FaceToAss : GadgetEffect
    {
        [SerializeField] private float during;



        protected override void CallWhenUse()
        {

            StartCoroutine(FaceToAssForSeconds(during));
        }

        IEnumerator FaceToAssForSeconds(float during)
        {
            model.SpeedGain = Mathf.Abs(model.SpeedGain) * -1;
            yield return new WaitForSeconds(during);
            model.SpeedGain = Mathf.Abs(model.SpeedGain);
            enabled = false;
        }
    }
}