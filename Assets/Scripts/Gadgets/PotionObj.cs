using UnityEngine;
using Gadget.Effecter;
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
    Transform avoidStop;
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

    public void Setting(Transform owner, Vector2 direction, int id)
    {
        ID = id;
        avoidStop = owner;
        //triggerMethod = trigger;

        Fly(direction, force);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform == avoidStop) return;
        zone.enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        body.enabled = false;
        time = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IEffecter>() != null)
        {
            other.GetComponent<IEffecter>().UseGadget(ID);

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