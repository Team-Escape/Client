using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameManagerSpace.Score
{
    public class View : MonoBehaviour
    {
        [SerializeField] float delaySec = 0.1f;
        [SerializeField] Sprite scoreSprite = null;

        public void Init(GameObject scoreUI, int currentScores)
        {
            for (int i = 0; i < currentScores; i++)
            {
                GameObject go = scoreUI.transform.GetChild(i + 1).gameObject as GameObject;
                Image image = go.GetComponent<Image>();
                image.sprite = scoreSprite;
            }
        }

        public IEnumerator SetUIToScore(GameObject scoreUI, int curretnScores, int newScores, System.Action<int> callback, int id)
        {
            while (curretnScores < newScores)
            {
                GameObject go = null;
                try
                {
                    go = scoreUI.transform.GetChild(curretnScores + 1).gameObject as GameObject;
                }
                catch
                {
                    if (callback != null)
                        callback(id);
                    yield break;
                }
                if (go != null)
                {
                    Image image = go.GetComponent<Image>();
                    image.sprite = scoreSprite;
                    curretnScores++;
                    yield return new WaitForSecondsRealtime(delaySec);
                }
            }
        }

    }
}