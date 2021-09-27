using UnityEngine;

using System.Collections;
namespace PlayerSpace.Gameplayer
{

    public class JumpLower : ItemBase
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
            model.jumpGain -= force;

            yield return new WaitForSeconds(delay);

            model.jumpGain += force;
            yield return null;
            isenable = false;
        }
    }
}