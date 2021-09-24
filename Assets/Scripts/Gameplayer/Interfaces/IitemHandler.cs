using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public interface IitemHandler
    {
        
        void SetGameItem(ItemData id);
        void Use();
        void EffectBy(ItemData itemID);
        Sprite GetCurrentSprite();
        bool isEmpty();
    }

}