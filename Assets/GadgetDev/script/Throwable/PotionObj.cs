using UnityEngine;
using Gadget.effecter;
using System.Collections;
using ObjectPool;
public class PotionObj : MonoBehaviour,IPoolObject {
    [SerializeField]Collider2D zone;
    [SerializeField]Collider2D body;
    [SerializeField]float staytime;
    [SerializeField]float force;
    [SerializeField]int ID;
    Transform avoidStop;
    CallWhenTrigger triggerMethod;
    float time;
    private void Awake() {
        time = -1;
    }
    public void Fly(Vector2 direction,float force){
        GetComponent<Rigidbody2D>().velocity = direction*force; 
    }
    public delegate void CallWhenTrigger(IEffecter effecter);
    public void SetTriggerMethod(CallWhenTrigger triggerMethod){
        this.triggerMethod = triggerMethod;
    }
    public void Setting(Transform owner,CallWhenTrigger trigger,Vector2 direction){
        avoidStop = owner;
        triggerMethod = trigger;
        Fly(direction,force);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform==avoidStop) return;
        zone.enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        body.enabled = false;
        time = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<IEffecter>()!=null){
            triggerMethod(other.GetComponent<IEffecter>());
            
        }
    }
    private void Update() {
        if(time!=-1&&Time.time>=time+staytime)
            GadgetPool.PutObject(gameObject);
    }
    // implement Ipoolobject
    public void Recycle(){
        gameObject.SetActive(false);
    }
    public void Init(){
        gameObject.SetActive(true);
    }
    public int GetID(){
        return ID;
    }
}