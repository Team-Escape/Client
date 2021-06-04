using UnityEngine;
using PlayerSpace.Game;

using System.Collections.Generic;

namespace Gadget.Effector
{
    public class Effector : MonoBehaviour, IEffector
    {
        Model playerModel;
        bool effect;
        

        [SerializeField]int MaxEffectCount = 10;
        [SerializeField]Camera camera;
        [SerializeField]GadgetEffect[] gadgets ;
        Dictionary<int,GadgetEffect> gadgetDict;
        [SerializeField]GameObject player;
        
        private void Awake()
        {
            gadgetDict = new Dictionary<int, GadgetEffect>();
            
            foreach (GadgetEffect item in GetComponents<GadgetEffect>())
            {
                
                gadgetDict.Add(item.GetId(),item);
            }
            gadgets = new GadgetEffect[gadgetDict.Count];
            gadgetDict.Values.CopyTo(gadgets,0);
            playerModel = GetComponent<Model>();
            effect = false;
            
        }
        public Camera GetCamera(){
            return camera;
        }
        public GameObject GetPlayer(){
            return player;
        }
        
        public void UseGadget(int gid){
            
            gadgetDict[gid].enabled = true;
        }
        public Sprite GetSprite(int gid){
            return gadgetDict[gid].GetSprite();
        }
    }
}