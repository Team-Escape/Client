using UnityEngine;

using System.Collections;
namespace PlayerSpace.Gameplayer
{
    public class SpeedCnange : ItemBase
    {
        [SerializeField] public float delay;
        [SerializeField] private float addition;

        protected override void CallWhenUse()
        {

            StartCoroutine(SpeedUpIEum());

        }
        IEnumerator SpeedUpIEum()
        {
            float speed = addition;
            model.speedGain =(Mathf.Abs(model.speedGain)+speed)*(Mathf.Sign(model.speedGain));

            yield return new WaitForSeconds(delay);

            model.speedGain = (Mathf.Abs(model.speedGain)-speed)*(Mathf.Sign(model.speedGain));
            yield return null;
            isenable = false;
        }
    }
}