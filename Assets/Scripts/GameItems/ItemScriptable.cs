using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    [CreateAssetMenu(menuName = "Items")]
    public class ItemScriptable : ScriptableObject
    {
        public int id;
        public string itemName;
        public ShrinkingPotion shrinkingPotion;
        public GameItemControl item;
        public Sprite sprite;
    }
}