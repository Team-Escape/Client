using UnityEngine;
using Gadget.Utility;
using ObjectPool;
public class GadgetSpawnController : MonoBehaviour
{
    [SerializeField] GameObject spawnGadget;
    [SerializeField] int id;
    [SerializeField] GameObject gadget;
    private void Awake()
    {

    }
    private void Start()
    {

        Spawn();
    }
    private void Update()
    {
        Spawn();

    }
    void Spawn()
    {
        if (gadget == null)
        {
            gadget = GadgetPool.GetObject(spawnGadget.GetComponent<IPoolObject>().GetPID());
            gadget.GetComponent<IGadget>().SetID(id);
            gadget.transform.position = transform.position;
            //GetComponent<SpriteRenderer>().sprite = gadget.GetComponent<IGadget>().GetIcon();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gadget == null) return;
        if (other.GetComponent<InteractWithGadget>() != null)
        {
            if (other.GetComponent<InteractWithGadget>().PickUp(gadget.GetComponent<IGadget>()))
            {
                this.gadget.transform.parent = other.gameObject.transform;
                this.gadget.transform.position = new Vector2(0, 0);
                this.gadget = null;
                //GetComponent<SpriteRenderer>().sprite =null;
            }

        }
    }

}