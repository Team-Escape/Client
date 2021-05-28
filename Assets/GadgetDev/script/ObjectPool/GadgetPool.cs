using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ObjectPool{
    public class GadgetPool:MonoBehaviour{
        [SerializeField]
        GameObject[] preObject;
        
        static GameObject statictMyself;
        Dictionary<int,Queue<GameObject>> list; 
        static Dictionary<int,Queue<GameObject>> staticlist = new Dictionary<int, Queue<GameObject>>(); 
        private void Awake() {
            list = new Dictionary<int, Queue<GameObject>>();
            statictMyself = gameObject;
            LoadObjects();
        }
        private void Start() {
            
        }
        private void LoadObjects(){
            foreach (GameObject curr in preObject)
            {  
                Queue<GameObject> queue = new Queue<GameObject>();
                
                for(int i=0;i<10;i++){
                    GameObject instance = Instantiate(curr,transform);
                    queue.Enqueue(instance);
                    instance.GetComponent<IPoolObject>().Recycle();
                }
                GadgetPool.staticlist.Add(curr.GetComponent<IPoolObject>().GetID(),queue);
            }
        }
        static public GameObject GetObject(int type){
            
            GameObject obj = GadgetPool.staticlist[type].Dequeue();
            obj.GetComponent<IPoolObject>().Init();
            
            return obj;
        }
        
        static public void PutObject(GameObject obj){
            int type = obj.GetComponent<IPoolObject>().GetID();
            obj.GetComponent<IPoolObject>().Recycle();
            obj.transform.parent = GadgetPool.statictMyself.transform;
            GadgetPool.staticlist[type].Enqueue(obj);
        }
    }
}