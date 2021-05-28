using UnityEngine;
using Gadget.effecter;
using PlayerSpace.Game;
using System.Collections;

namespace Gadget.utility
{
    public class speedUp : GadgetBase
    {
        [SerializeField] public float delay;
        [SerializeField] private float addition;
        Model model;
        protected override void CallWhenUse(IEffecter getEffect)
        {
            model = getEffect.GetModel();
            StartCoroutine(SpeedUpIEum());

        }
        IEnumerator SpeedUpIEum()
        {
            model.ItemSpeedGain += addition;

            yield return new WaitForSeconds(delay);

            model.ItemSpeedGain -= addition;
            yield return null;
        }
    }
}