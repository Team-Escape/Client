using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ObjectPool{
    public class GadgetPool:MonoBehaviour{
        [SerializeField]
        GameObject[] preObject;
        [SerializeField] int count;
        static GameObject statictMyself;
        static Dictionary<int,Queue<GameObject>> staticlist = new Dictionary<int, Queue<GameObject>>(); 
        private void Awake() {
            statictMyself = gameObject;
            LoadObjects();
        }
        private void Start() {
            
        }
        private void LoadObjects(){
            if(transform.childCount>0){
                foreach(Transform child in transform){
                    Destroy(child.gameObject);
                }
            }
            GadgetPool.staticlist.Clear();
            foreach (GameObject curr in preObject)
            {  
                Queue<GameObject> queue = new Queue<GameObject>();
                
                for(int i=0;i<count;i++){
                    GameObject instance = Instantiate(curr,transform);
                    queue.Enqueue(instance);
                    instance.GetComponent<IPoolObject>().Recycle();
                }
                GadgetPool.staticlist.Add(curr.GetComponent<IPoolObject>().GetPID(),queue);
            }
        }
        static public GameObject GetObject(int type){
            if(GadgetPool.staticlist[type].Count<=0)return null;
            GameObject obj = GadgetPool.staticlist[type].Dequeue();
            obj.GetComponent<IPoolObject>().Init();
            
            return obj;
        }
        
        static public void PutObject(GameObject obj){
            int type = obj.GetComponent<IPoolObject>().GetPID();
            obj.GetComponent<IPoolObject>().Recycle();
            obj.transform.parent = GadgetPool.statictMyself.transform;
            GadgetPool.staticlist[type].Enqueue(obj);
        }
    }
}