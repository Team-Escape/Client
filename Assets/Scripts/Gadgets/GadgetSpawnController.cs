using UnityEngine;
using Gadget.Utility;
using ObjectPool;
using Gadget.Effector;



public class GadgetSpawnController : MonoBehaviour
{
    [SerializeField] GameObject effector;
    [SerializeField] int size;
    [SerializeField] GameObject[] spawnGadget;
    [SerializeField] int[] id;
    [SerializeField] bool[] effectToOwner;
   
    
    [SerializeField] bool loadAllEffect;
    [SerializeField] bool isUsefunctionRandom;
    [SerializeField] bool isEffectToOwnerRandom;

    [SerializeField] int currId;
    [SerializeField] GameObject gadget;
    [SerializeField] bool curreffectToOwner;
    
    private void Awake()
    {
        
        effector.GetComponent<InteractWithGadget>().enabled = false;
        effector.SetActive(false);
    }
    private void Start()
    {
        LoadEffects();
        Spawn();
    }
    private void Update()
    {
        Spawn();

    }
    void LoadEffects(){
        if(!loadAllEffect) return;
        
        GadgetEffect[] effects = effector.GetComponent<IEffector>().GetGadgetEffects();
        size = effects.Length;
        id = new int[size];
        for(int i=0;i<size;i++){
            id[i] = effects[i].GetId();
        }
        
    }
    void Spawn()
    {
        if(spawnGadget.Length==0||id.Length==0
        ||this.effectToOwner.Length==0&&!isEffectToOwnerRandom) return;
        if (gadget == null)
        {
            //int index = Random.Range(0,size);
            
            int idIndex = Random.Range(0,id.Length);
            int spawnGadgetIndex = idIndex;
            int effectToOwnerIndex = idIndex;
            bool effectToOwner;
            if(isUsefunctionRandom)spawnGadgetIndex = Random.Range(0,spawnGadget.Length);
            if(isEffectToOwnerRandom){
                effectToOwner = Random.Range(0,2)==1;
            }
            else{
                effectToOwner = this.effectToOwner[effectToOwnerIndex];
            }
            
            gadget = GadgetPool.GetObject(spawnGadget[spawnGadgetIndex].GetComponent<IPoolObject>().GetPID());
            gadget.GetComponent<IGadget>().InitGadget(id[idIndex], effectToOwner);
            gadget.transform.position = transform.position;
            
            GetComponent<SpriteRenderer>().sprite =effector.GetComponent<IEffector>().GetSprite(id[idIndex]);
            currId = id[idIndex];
            curreffectToOwner = effectToOwner;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (gadget == null) return;
        if (other.GetComponent<InteractWithGadget>() != null)
        {
            Debug.Log("get");
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