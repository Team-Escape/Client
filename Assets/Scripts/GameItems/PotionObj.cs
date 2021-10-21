using UnityEngine;
using ObjectPool;
using PlayerSpace.Gameplayer;
public class PotionObj : MonoBehaviour, IPoolObject,ItemObj
{
    [SerializeField] Collider2D zone;
    [SerializeField] Collider2D body;
    [SerializeField] float staytime;
    [SerializeField] float force;

    [SerializeField] int pid;
    [SerializeField]ItemData itemData;
    Transform ownertrans;
    bool isEffectOwner;
    //CallWhenTrigger triggerMethod;
    string ownerHash;
    float time;
    public ItemData GetItemData(){
        return itemData;
    }
    public void Fly(Vector2 direction, float force)
    {
        this.transform.position = ownertrans.position;
        GetComponent<Rigidbody2D>().velocity = direction * force;
    }
    //public delegate void CallWhenTrigger(IEffecter effecter);

    public void Setting(Transform owner, ItemData itemData,string ownerHash)
    {
        this.ownerHash = ownerHash;
        this.itemData = itemData;
        this.ownertrans = owner;
        //this.isEffectOwner = isEffectOwner;
        //triggerMethod = trigger;
        zone.enabled = false;
        body.enabled = true;
        
        
        Fly(owner.localScale, force);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.Equals(ownertrans)||other.gameObject.tag=="GameItem") return;
        zone.enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        body.enabled = false;
        time = Time.time;
    }
    public string GetOwnerHash(){
        return ownerHash;
    }
    private void Update()
    {
        if (time != -1 && Time.time >= time + staytime)
            GadgetPool.PutObject(gameObject);
    }
    // implement Ipoolobject
    public void Recycle()
    {
        time = -1;
        zone.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        body.enabled = true;
        gameObject.SetActive(false);
    }
    public void Init()
    {
        gameObject.SetActive(true);
    }

    public int GetPID()
    {
        return pid;
    }
    
}