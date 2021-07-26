using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class ItemModel : MonoBehaviour
    {
        public static ItemModel instance;
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
        public GameItemControl GetItem(int id) => items[id].GetComponent<GameItemControl>();
    }
}