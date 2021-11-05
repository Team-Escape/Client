namespace PlayerSpace.Gameplayer
{
    using UnityEngine;
    using System;

    
    public class ItemSpawner : MonoBehaviour,IitemSpawner {
        [Header("GameObject linking")]
        [SerializeField] GameObject effector;
        [SerializeField]GameObject testingUI;
        [Header("Setting")]
        [SerializeField] bool loadAllEffect;
        [SerializeField] ItemEffect[] effectIds;
        [SerializeField] bool isActionRelatedWithEffect;
        [SerializeField] ItemAction[] actions;
        [SerializeField] bool isEffectToOwnerRelatedWithEffect;
        [SerializeField] bool[] effectToOwners;
        
        [SerializeField] float spawnTime;
        [SerializeField]Sprite defaultSprite ;
        [Header("Runtime")]
        [SerializeField]ItemData currItem;
        [SerializeField]float currentCounter;
        [SerializeField]bool isEmptyLog; 

        [SerializeField]bool testingMode;
        ItemData NULLItem = new ItemData();
        private void Awake()
        {
            //testingUI.SetActive(testingMode);
            defaultSprite = GetComponent<SpriteRenderer>().sprite;
            //effector.GetComponent<InteractWithGadget>().enabled = false;
            //effector.SetActive(false);
            currentCounter = Time.time;
            currItem = NULLItem;
        }
        private void Start()
        {
            loadEffects();
            spawn();
        }
        private void Update()
        {
            if(!check_Setting_is_legal()) return;
            if (IsEmpty() && Time.time-currentCounter>=spawnTime){
                spawn();
            }
            isEmptyLog = IsEmpty();
        }
        void loadEffects(){
            if(!loadAllEffect) return;
            
            Array itemEffects =Enum.GetValues(typeof(ItemEffect));
            int size = itemEffects.Length-1;
            effectIds = new ItemEffect[size];
            for(int i=0;i<size;i++){
                ItemEffect getEffect = (ItemEffect)itemEffects.GetValue(i);
                if(getEffect==ItemEffect.NULL) continue;
                effectIds[i] = getEffect;
            }
            
        }
        public void spawn()
        {
            setCurrentItem();
            setSprite(defaultSprite);
        }
        bool check_Setting_is_legal(){
            bool islegal = !(actions.Length==0||effectIds.Length==0
            ||this.effectToOwners.Length==0&&isEffectToOwnerRelatedWithEffect);
            if(!islegal) Debug.Log("setting not legal");
            return islegal;
        }
        void setSprite(Sprite sprite){
            GetComponent<SpriteRenderer>().sprite  = sprite;
        }
        void setCurrentItem(){
                int idIndex = UnityEngine.Random.Range(0,effectIds.Length);
                int actionIndex = idIndex;
                int effectToOwnerIndex = idIndex;
                if(!isActionRelatedWithEffect)actionIndex = UnityEngine.Random.Range(0,actions.Length);
                if(!isEffectToOwnerRelatedWithEffect){
                    effectToOwnerIndex = UnityEngine.Random.Range(0,this.effectToOwners.Length);
                }
                currItem = 
                new ItemData(
                    actions[actionIndex],
                    effectIds[idIndex],
                    effectToOwners[effectToOwnerIndex]
                );
                
        }
        Sprite searchSpriteFromDatabase(ItemData item){
            return effector.GetComponent<ItemSystem>().GetItemSprite(item);
        }
        public ItemData TackItem(){
            ItemData outData = currItem;
            Reset();
            return outData;
        }
        public void Reset(){
            currItem = NULLItem;
            currentCounter = Time.time;
            setSprite(null);
        }
        public bool IsEmpty(){
            return currItem==NULLItem;
        }
    }
    
}