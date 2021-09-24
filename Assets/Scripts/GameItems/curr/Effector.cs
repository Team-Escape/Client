using UnityEngine;
using PlayerSpace.Game;

using System.Collections.Generic;

namespace PlayerSpace.Gameplayer
{
    public class Effector : MonoBehaviour,ItemSystem
    {
        Model playerModel;
        bool effect;
        

        //[SerializeField]int MaxEffectCount = 10;
        
        [SerializeField]ItemEffecActor[] gadgets ;
        Dictionary<ItemEffect,ItemEffecActor> gadgetDict;
        
        [SerializeField]Sprite sprite;
        private void Awake()
        {
            gadgetDict = new Dictionary<ItemEffect, ItemEffecActor>();
            
            foreach (ItemEffecActor item in GetComponents<ItemEffecActor>())
            {
                
                gadgetDict.Add(item.GetId(),item);
            }
            gadgets = new ItemEffecActor[gadgetDict.Count];
            gadgetDict.Values.CopyTo(gadgets,0);
            //playerModel = GetComponent<Model>();
            effect = false;
            
        }
        
        public void UseItem(ItemData itemdata){
            //Debug.Log("Use"+itemdata);
            EffectBy(itemdata);
        }
        //get sprite from item
        public Sprite GetItemSprite(ItemData itemdata){
            //Debug.Log("GetItemSprite "+itemdata);
            return sprite;
        }
        public void EffectBy(ItemData itemdata){
            //Debug.Log("EffectBy"+itemdata);
            gadgetDict[itemdata.effect].Trigger();
        }
        public void SetPlayerModel(Model model){
            playerModel = model;
        }
    }
}