using UnityEngine;
using PlayerSpace.Game;

using System.Collections.Generic;

namespace Gadget.Effector
{
    public class Effector : MonoBehaviour, IEffector
    {
        Model playerModel;
        bool effect;

        HashSet<int> effs;
        [SerializeField] int MaxEffectCount = 10;
        [SerializeField] Camera cam;
        [SerializeField] GadgetEffect[] gadgets;

        [SerializeField] GameObject player;
        public GameObject GetPlayer()
        {
            return player;
        }
        private void Awake()
        {
            gadgets = GetComponents<GadgetEffect>();
            effs = new HashSet<int>();

            playerModel = GetComponent<Model>();
            effect = false;

        }
        public Camera GetCamera()
        {
            return cam;
        }
        public Model GetModel()
        {
            return playerModel;
        }
        public bool SetEffect(int id)
        {
            if (!effs.Contains(id))
            {
                effs.Add(id);
                return true;
            }
            return false;
        }
        public void DistoryEffect(int id)
        {
            effs.Remove(id);
        }
        public void UseGadget(int gid)
        {

            gadgets[gid].enabled = true;
        }
        public Sprite GetSprite(int gid)
        {
            return gadgets[gid].GetSprite();
        }
    }
}