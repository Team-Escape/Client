using UnityEngine;
using Gadget.Effecter;
using System.Collections;
using PlayerSpace.Game;
namespace Gadget.Utility
{
    public class Invisible : GadgetEffect
    {
        [SerializeField] private float during;

        int invisibleLayer;
        int defultLayer;
        int defultLayerMask;

        protected override void CallWhenUse()
        {
            //getEffect.UseInvisible(during);
            defultLayerMask = cam.cullingMask;
            invisibleLayer = LayerMask.NameToLayer("Invisible");
            defultLayer = owner.layer;

            StartCoroutine(InvisibleForSeconds(during));
        }

        IEnumerator InvisibleForSeconds(float during)
        {
            cam.cullingMask = cam.cullingMask | (1 << invisibleLayer);
            owner.layer = invisibleLayer;
            yield return new WaitForSeconds(during);
            cam.cullingMask = defultLayerMask;
            owner.layer = defultLayer;
            enabled = false;
        }
    }
}