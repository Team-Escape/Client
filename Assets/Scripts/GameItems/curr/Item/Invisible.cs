using UnityEngine;

using System.Collections;

namespace PlayerSpace.Gameplayer
{
    public class Invisible : ItemBase
    {
        [SerializeField] private float during;

        int invisibleLayer;
        int defultLayer;
        int defultLayerMask;
        // 要改玩家自己的碰撞與shader
        protected override void CallWhenUse()
        {

            defultLayerMask = model.cam.cullingMask;
            invisibleLayer = LayerMask.NameToLayer("Invisible");
            //defultLayer = owner.layer;

            StartCoroutine(InvisibleForSeconds(during));
        }

        IEnumerator InvisibleForSeconds(float during)
        {
            model.cam.cullingMask = model.cam.cullingMask | (1 << invisibleLayer);
            //owner.layer = invisibleLayer;
            yield return new WaitForSeconds(during);
            model.cam.cullingMask = defultLayerMask;
            //owner.layer = defultLayer;
            isenable = false;
        }
    }
}