using UnityEngine;
using System.Collections.Generic;
namespace PlayerSpace.Gameplayer
{
    public interface ItemSystemManager
    {
        //return a hash of item
        int GetItem(int id,Model playerModel);
        //use a item with the hash
        void UseItem(int itemHash);
        //get sprite from item
        Sprite GetItemSprite(int itemHash);
        //get all item Names and id 
        IDictionary<int,string> getNameTable(); 
    }
}