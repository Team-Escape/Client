using UnityEngine;
using PlayerSpace.Game;
using System.Collections;
namespace Gadget.effecter
{
    public class Effecter : MonoBehaviour, IEffecter
    {
        Model playerModel;
        int invisibleLayer;
        bool effect;
        int defultLayer;
        int defultLayerMask;
        [SerializeField] Camera cam = null;
        [SerializeField] GameObject myself = null;
        private void Awake()
        {
            defultLayerMask = cam.cullingMask;
            invisibleLayer = LayerMask.NameToLayer("Invisible");
            playerModel = GetComponent<Model>();
            effect = false;
            defultLayer = myself.layer;
        }

        public void UseHeal(int healMax, int healperTime, float delay)
        {
            if (!effect)
                StartCoroutine(addHeal(healMax, healperTime, delay));
        }
        public void UseInvisible(float during)
        {
            if (!effect)
            {
                StartCoroutine(InvisibleForSeconds(during));
            }
        }
        public void UseReduction(float during, float smallerScale, float stay, float limitScale)
        {
            if (!effect)
            {
                StartCoroutine(ReductionForSeconds(during, smallerScale, stay, limitScale));
            }
        }
        IEnumerator addHeal(int healMax, int healperTime, float delay)
        {
            effect = true;
            for (int currHeal = 0; currHeal < healMax; currHeal += healperTime)
            {
                if (playerModel.CurrentHealth < playerModel.MaxHealth)
                {
                    if (healMax - (currHeal + healperTime) < 0)
                    {
                        playerModel.CurrentHealth += healMax - currHeal;
                    }
                    else
                    {
                        playerModel.CurrentHealth += healperTime;
                    }
                }
                yield return new WaitForSeconds(delay);

            }
            effect = false;
        }
        IEnumerator InvisibleForSeconds(float during)
        {
            cam.cullingMask = cam.cullingMask | (1 << invisibleLayer);
            myself.layer = invisibleLayer;
            yield return new WaitForSeconds(during);
            cam.cullingMask = defultLayerMask;
            myself.layer = defultLayer;
        }
        IEnumerator ReductionForSeconds(float during, float smallerScale, float stay, float limitScale)
        {
            float defultScale = playerModel.CharacterSize;
            int i = 1;
            while (Mathf.Pow(limitScale, i) > smallerScale)
            {
                playerModel.CharacterSize = defultScale * Mathf.Pow(limitScale, i);
                i++;
                yield return new WaitForSeconds(during);
            }
            yield return new WaitForSeconds(stay);
            while (i > 0)
            {
                playerModel.CharacterSize = defultScale * Mathf.Pow(limitScale, i);
                i--;
                yield return new WaitForSeconds(during);
            }
            playerModel.CharacterSize = defultScale;
        }

        public Model GetModel()
        {
            return playerModel;
        }




    }
}