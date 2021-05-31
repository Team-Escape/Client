using UnityEngine;
using UnityEngine.UI;
using Gadget.Effecter;
namespace Gadget.Utility
{
    public interface IGadget
    {
        int GetID();
        Sprite GetIcon();
        int GetObjType();
        void Use(GameObject owner);
        void SetID(int id);
    }
}