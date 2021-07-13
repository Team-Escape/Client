using UnityEngine;
using UnityEngine.UI;
using Gadget.Effector;
namespace Gadget.Utility
{
    public interface IGadget
    {
        int GetID();
        Sprite GetIcon();
        int GetObjType();
        void Use(GameObject owner);
        void InitGadget(int id, bool isEffectOwner);

    }
}