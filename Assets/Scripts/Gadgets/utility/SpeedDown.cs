using UnityEngine;
using Gadget.Effector;
using PlayerSpace.Game;
using System.Collections;
namespace Gadget.Utility
{
    public class SpeedDown : GadgetEffect
    {
        [SerializeField] public float delay;
        [SerializeField] private float addition;

        protected override void CallWhenUse()
        {

            StartCoroutine(SpeedDownIEum());

        }
        IEnumerator SpeedDownIEum()
        {
            float speed = addition;
            model.SpeedGain += speed;

            yield return new WaitForSeconds(delay);

            model.SpeedGain -= speed;
            yield return null;
            enabled = false;
        }
    }
}
