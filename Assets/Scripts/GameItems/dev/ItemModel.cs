using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class ItemModel : MonoBehaviour,ItemSystemManager
    {
        [System.Serializable]
        public struct GameItemEntity{
            
            public string name;
            public GameObject itemObj;
            public Sprite sprite;
        } 
        private List<IGameItemControl> pool;
        private Dictionary<int,GameItemEntity> items;
        private int currentPoolIndex = 0;//set max index
        [SerializeField] List<GameItemEntity> itemSerializeView;
        //singleton
        public static ItemModel instance;

        public void Recycle(IGameItemControl gameItemControl){
            pool.Remove(gameItemControl);
        }
        //instant a item by id
        private IGameItemControl InstantItem(int id)
        {
            GameObject newItem = Instantiate(items[id].itemObj);
            IGameItemControl itemControl = newItem.GetComponent<IGameItemControl>();
            return itemControl;
        }
        private void LoadSerializeIntoDictionary(){
            int index = 0;
            foreach(var item in itemSerializeView){
                items.Add(index,item);
                index++;
            }
        }
        private int PutItemInPool(IGameItemControl item){
            currentPoolIndex++;
            pool.Insert(currentPoolIndex,item);
            return currentPoolIndex;
        }
        #region unity api
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                LoadSerializeIntoDictionary();
            }
            else
                Destroy(gameObject);
        }
        #endregion
        #region implement of ItemSystemManager
        //return a hash of item
        public int GetItem(int id,Model playerModel)
        {
            IGameItemControl itemControl = InstantItem(id);
            itemControl.Init(playerModel);
            return PutItemInPool(itemControl);
        }

        //use a item with the hash
        public void UseItem(int itemHash){
            pool[itemHash].Use();
        }
        //get sprite from item
        public Sprite GetItemSprite(int itemId){
            return items[itemId].sprite;
        }
        public IDictionary<int,string> getNameTable(){
            IDictionary<int,string> dic = new Dictionary<int, string>();
            foreach (var item in items)
            {
                dic.Add(item.Key,item.Value.name);
            }
            return dic;
        }
        #endregion
        
    }   
}