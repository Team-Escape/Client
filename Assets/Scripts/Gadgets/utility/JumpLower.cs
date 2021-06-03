using UnityEngine;
using Gadget.Effector;
using System.Collections;
namespace Gadget.Utility
{

    public class JumpLower : GadgetEffect
    {
        [SerializeField] public float delay;
        [SerializeField] private float divition;

        protected override void CallWhenUse()
        {

            StartCoroutine(JumpLowerIEum());

        }
        IEnumerator JumpLowerIEum()
        {
            float force = divition;
            model.JumpGain -= force;

            yield return new WaitForSeconds(delay);

            model.JumpGain += force;
            yield return null;
            enabled = false;
        }
    }
}