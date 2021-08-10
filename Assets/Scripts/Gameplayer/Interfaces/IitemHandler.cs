using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public interface IitemHandler
    {
        
        void SetGameItem(int id, Model playerModel);
        void Use();
        void EffectBy(int itemID);
        Sprite GetCurrentSprite();
        bool isEmpty();
    }

}