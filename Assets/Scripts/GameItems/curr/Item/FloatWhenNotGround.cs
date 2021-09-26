
using UnityEngine;

using System.Collections;
namespace PlayerSpace.Gameplayer
{
    //not complete
    public class FloatWhenNotGround : ItemBase
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
            float originSpeed = model.speedGain;
            if (false)
                model.speedGain -= fallingDivition;
            else

                yield return new WaitForSeconds(delay);

            model.jumpGain += force;
            yield return null;
            isenable = false;
        }
    }
}