using UnityEngine;
using Gadget.Effecter;
using PlayerSpace.Game;
using System.Collections;

namespace Gadget.Utility
{
    public class SpeedUp : GadgetEffect
    {
        [SerializeField] public float delay;
        [SerializeField] private float addition;

        protected override void CallWhenUse()
        {

            StartCoroutine(SpeedUpIEum());

        }
        IEnumerator SpeedUpIEum()
        {
            model.ItemSpeedGain += addition;

            yield return new WaitForSeconds(delay);

            model.ItemSpeedGain -= addition;
            yield return null;
            enabled = false;
        }
    }
}