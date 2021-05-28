using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameManagerSpace.Award
{
    public class AwardManager : MonoBehaviour
    {
        [SerializeField] TestEnviorment tester = null;
        [SerializeField] GameObject[] actors = null;
        System.Action<string> loadSceneAction;

        public void Init(System.Action<string> loadSceneActionCallback)
        {
            loadSceneAction = loadSceneActionCallback;
        }

        public void ChangeScene()
        {
            loadSceneAction("HallScene");
        }

        private void Awake()
        {
            if (tester.doTest)
            {
                ActorsDressup(tester.testPrefab);
            }
            else
            {
                ActorsDressup(CoreModel.WinnerAvatars);
            }
        }

        void ActorsDressup(List<GameObject> avatars)
        {
            for (int i = 0; i < actors.Length; i++)
            {
                Animator director = avatars[i].GetComponentInChildren<Animator>();
                actors[i].GetComponent<Animator>().runtimeAnimatorController = director.runtimeAnimatorController;
            }
        }
    }

    [System.Serializable]
    public class TestEnviorment
    {
        public bool doTest = false;
        public List<GameObject> testPrefab = null;
    }

}