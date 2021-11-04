using UnityEngine;

using ObjectPool;
using System.Collections.Generic;

namespace PlayerSpace.Gameplayer
{
    public class Effector : MonoBehaviour,ItemSystem
    {
        Model playerModel;
        [SerializeField]bool effect;
        

        //[SerializeField]int MaxEffectCount = 10;
        //[SerializeField] GameObject throwItem;
        ItemEffecActor[] gadgets ;
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
            
            effect = false;
            
        }
        
        public void UseItem(ItemData itemdata){
            if(itemdata.action==ItemAction.EAT)
                gadgetDict[itemdata.effect].Trigger(playerModel);
            else if(itemdata.action==ItemAction.THROW){
                GameObject go = GadgetPool.GetObject(1);
                if(go==null) return;
                go.GetComponent<PotionObj>().Setting(
                    playerModel.selfTransform,
                    itemdata,
                    playerModel.GetHashCode().ToString()
                );
                //Instantiate(throwItem,playerModel.selfTransform);
            }
        }
        //get sprite from item
        public Sprite GetItemSprite(ItemData itemdata){
            
            return gadgetDict[itemdata.effect].GetSprite();
        }
        public void EffectBy(ItemObj itemobj){
            ItemData data = itemobj.GetItemData();
            if(!data.isAffectOwner&&
            itemobj.GetOwnerHash()==playerModel.GetHashCode().ToString())
                return;
            gadgetDict[data.effect].Trigger(playerModel);
            
        }
        public void SetPlayerModel(Model model){
            playerModel = model;
        }
    }
}