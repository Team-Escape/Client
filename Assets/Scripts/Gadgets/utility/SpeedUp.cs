using UnityEngine;
using Gadget.Effector;
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
            float speed = addition;
            model.SpeedGain += speed;

            yield return new WaitForSeconds(delay);

            model.SpeedGain -= speed;
            yield return null;
            enabled = false;
        }
    }
}