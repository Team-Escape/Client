using UnityEngine;
using Gadget.Effector;
using PlayerSpace.Game;
using System.Collections;
namespace Gadget.Utility
{
    public class SpeedCnange : GadgetEffect
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
            model.SpeedGain =(Mathf.Abs(model.SpeedGain)+speed)*(Mathf.Sign(model.SpeedGain));

            yield return new WaitForSeconds(delay);

            model.SpeedGain = (Mathf.Abs(model.SpeedGain)-speed)*(Mathf.Sign(model.SpeedGain));
            yield return null;
            enabled = false;
        }
    }
}