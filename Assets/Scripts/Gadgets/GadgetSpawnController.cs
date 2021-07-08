using UnityEngine;
using Gadget.Utility;
using ObjectPool;
using Gadget.Effector;



public class GadgetSpawnController : MonoBehaviour
{
    [Header("GameObject linking")]
    [SerializeField] GameObject effector;
    [Header("Setting")]
    [SerializeField] bool loadAllEffect;
    [SerializeField] int[] effectId;
    [SerializeField] bool isSpawnGadgetRelatedWithEffectId;
    [SerializeField] GameObject[] spawnGadget;
    [SerializeField] bool isEffectToOwnerRelatedWithEffectId;
    [SerializeField] bool[] effectToOwner;
    
    [SerializeField] float delayWhenTakeOut;
    
    
    [Header("Runtime")]
    [SerializeField] int currId;
    [SerializeField] GameObject gadget;
    [SerializeField] bool curreffectToOwner;
    
    [SerializeField]float currentCounter;
    Sprite sprite ;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        effector.GetComponent<InteractWithGadget>().enabled = false;
        effector.SetActive(false);
        currentCounter = Time.time;
    }
    private void Start()
    {
        LoadEffects();
        Spawn();
    }
    private void Update()
    {
        
        if (gadget == null&&Time.time-currentCounter>=delayWhenTakeOut){
            Spawn();
        }
        

    }
    void LoadEffects(){
        if(!loadAllEffect) return;
        
        GadgetEffect[] effects = effector.GetComponent<IEffector>().GetGadgetEffects();
        int size = effects.Length;
        effectId = new int[size];
        for(int i=0;i<size;i++){
            effectId[i] = effects[i].GetId();
        }
        
    }
    void Spawn()
    {
        if(spawnGadget.Length==0||effectId.Length==0
        ||this.effectToOwner.Length==0&&isEffectToOwnerRelatedWithEffectId) return;
        if (gadget == null)
        {
            
            
            int idIndex = Random.Range(0,effectId.Length);
            int spawnGadgetIndex = idIndex;
            int effectToOwnerIndex = idIndex;
            
            if(!isSpawnGadgetRelatedWithEffectId)spawnGadgetIndex = Random.Range(0,spawnGadget.Length);
            if(!isEffectToOwnerRelatedWithEffectId){
                effectToOwnerIndex = Random.Range(0,this.effectToOwner.Length);
            }
            
            
            
            
            gadget = GadgetPool.GetObject(spawnGadget[spawnGadgetIndex].GetComponent<IPoolObject>().GetPID());
            gadget.GetComponent<IGadget>().InitGadget(effectId[idIndex], effectToOwner[effectToOwnerIndex]);
            gadget.transform.position = transform.position;
            
            GetComponent<SpriteRenderer>().sprite =sprite;//effector.GetComponent<IEffector>().GetSprite(effectId[idIndex]);
            currId = effectId[idIndex];
            curreffectToOwner = effectToOwner[effectToOwnerIndex];
            
        }
    }
/*
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
                currentCounter = Time.time;
                GetComponent<SpriteRenderer>().sprite =null;
            }

        }
    }*/
    private void OnTriggerStay2D(Collider2D other) {
        if (gadget == null) return;
        if (other.GetComponent<InteractWithGadget>() != null)
        {
            Debug.Log("get");
            if (other.GetComponent<InteractWithGadget>().PickUp(gadget.GetComponent<IGadget>()))
            {
                
                this.gadget.transform.parent = other.gameObject.transform;
                this.gadget.transform.position = new Vector2(0, 0);
                this.gadget = null;
                currentCounter = Time.time;
                GetComponent<SpriteRenderer>().sprite =null;
            }

        }
    }
}