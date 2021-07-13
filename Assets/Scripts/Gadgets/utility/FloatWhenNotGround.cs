
using UnityEngine;
using Gadget.Effector;
using System.Collections;
namespace Gadget.Utility
{
    //not complete
    public class FloatWhenNotGround : GadgetEffect
    {
        [SerializeField] public float delay;
        [SerializeField] private float fallingDivition;

        protected override void CallWhenUse()
        {

            StartCoroutine(JumpLowerIEum());

        }
        IEnumerator JumpLowerIEum()
        {
            float force = fallingDivition;
            float originSpeed = model.SpeedGain;
            if (model.Grounded)
                model.SpeedGain -= fallingDivition;
            else

                yield return new WaitForSeconds(delay);

            model.JumpGain += force;
            yield return null;
            enabled = false;
        }
    }
}