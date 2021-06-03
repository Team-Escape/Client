using UnityEngine;
using Gadget.Effector;
using System.Collections;
using ObjectPool;

public class PotionObj : MonoBehaviour, IPoolObject
{
    [SerializeField] Collider2D zone;
    [SerializeField] Collider2D body;
    [SerializeField] float staytime;
    [SerializeField] float force;
    [SerializeField] int ID;
    [SerializeField] int pid;
    Transform owner;
    bool isEffectOwner;
    //CallWhenTrigger triggerMethod;

    float time;
    private void Awake()
    {

    }
    public void Fly(Vector2 direction, float force)
    {
        GetComponent<Rigidbody2D>().velocity = direction * force;
    }
    //public delegate void CallWhenTrigger(IEffecter effecter);

    public void Setting(Transform owner, int id, bool isEffectOwner)
    {
        ID = id;
        this.owner = owner;
        this.isEffectOwner = isEffectOwner;
        //triggerMethod = trigger;
        //zone.enabled = false;
        //body.enabled = false;
        Fly(owner.transform.localScale, force);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.Equals(owner)) return;
        zone.enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        body.enabled = false;
        time = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (zone.enabled == false) return;

        if (other.GetComponent<IEffector>() != null)
        {
            IEffector eff = other.GetComponent<IEffector>();
            if (!isEffectOwner &&
            eff.GetPlayer().transform.Equals(owner))
            {
                return;
            }
            eff.UseGadget(ID);

        }
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
    public void SetID(int id)
    {
        ID = id;
    }
}