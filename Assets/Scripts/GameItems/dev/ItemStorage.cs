using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class ItemStorage: MonoBehaviour,ItemSystemManager
    {
        [SerializeField] GameItemContainer container;
        public static ItemStorage instance;
        public GameItemContainer LoadAllItems(){
            return Instantiate(container);
        }
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
                Destroy(gameObject);
        }
        public List<GameObject> items;
        public int GetItem(int id,Model playerModel)
        {
            var newItem = GetInstant(id);
            return 1;//newItem.GetComponent<IGameItemControl>();
        }

        private GameObject GetInstant(int id)
        {
            return Instantiate(items[id]);
        }
        //use a item with the hash
        public void UseItem(int itemHash){

        }
        //get sprite from item
        public Sprite GetItemSprite(int itemHash){
            return container.items[itemHash].sprite;
        }
        public IDictionary<int,string> getNameTable(){
            IDictionary<int,string> dic = new Dictionary<int, string>();
            
            return dic;
        }
    }  
}