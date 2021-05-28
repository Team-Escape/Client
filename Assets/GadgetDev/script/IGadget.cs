using UnityEngine;
using UnityEngine.UI;
using Gadget.effecter;
namespace Gadget.utility{
    public interface IGadget
    {
        void Use(IEffecter getEffet);
        Sprite GetIcon();
        int GetObjType();
    }
}