using UnityEngine;

using System.Collections;
namespace PlayerSpace.Gameplayer
{
    public class SpeedDown : ItemBase
    {
        [SerializeField] public float delay;
        [SerializeField] private float addition;

        protected override void CallWhenUse()
        {

            StartCoroutine(SpeedDownIEum());

        }
        IEnumerator SpeedDownIEum()
        {
            float speed = -addition;
            model.speedGain =(Mathf.Abs(model.speedGain)+speed)*(Mathf.Sign(model.speedGain));

            yield return new WaitForSeconds(delay);

            model.speedGain = (Mathf.Abs(model.speedGain)-speed)*(Mathf.Sign(model.speedGain));
            yield return null;
            isenable = false;
        }
    }
}