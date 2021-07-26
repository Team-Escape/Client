using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public interface ISlot
    {
        GameItemControl GameItemControl { get; set; }
        void SetGameItem(int id, Model playerModel);
        void Use();
    }

}