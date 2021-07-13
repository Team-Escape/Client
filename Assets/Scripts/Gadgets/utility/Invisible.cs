using UnityEngine;

using System.Collections;

namespace Gadget.Utility
{
    public class Invisible : GadgetEffect
    {
        [SerializeField] private float during;

        int invisibleLayer;
        int defultLayer;
        int defultLayerMask;
        // 要改玩家自己的碰撞與shader
        protected override void CallWhenUse()
        {

            defultLayerMask = camera.cullingMask;
            invisibleLayer = LayerMask.NameToLayer("Invisible");
            defultLayer = owner.layer;

            StartCoroutine(InvisibleForSeconds(during));
        }

        IEnumerator InvisibleForSeconds(float during)
        {
            camera.cullingMask = camera.cullingMask | (1 << invisibleLayer);
            owner.layer = invisibleLayer;
            yield return new WaitForSeconds(during);
            camera.cullingMask = defultLayerMask;
            owner.layer = defultLayer;
            enabled = false;
        }
    }
}