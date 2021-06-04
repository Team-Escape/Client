using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

namespace GameManagerSpace.Score
{
    public class ScoreManager : MonoBehaviour
    {
        public System.Action<string> changeGameStateAction = null;
        public System.Action scoreFinishedAction = null;

        [SerializeField] Transform scoreObject = null;
        [SerializeField] string nameOfScoreDisplay = "ScoreDisplay";

        View view = null;
        Model model = new Model();

        List<GameObject> goalPlayers = new List<GameObject>();
        Camera gameSceneMainCamera = null;

        public void Init(int n, List<int> obtainScores, System.Action<string> callback)
        {
            ActivePlayingScore(n, CoreModel.TotalScores);
            changeGameStateAction = callback;
            model.ObtainScores = obtainScores;
        }

        public void ActivePlayingScore(int n, List<int> currentScores)
        {
            int index = 0;
            foreach (Transform go in scoreObject)
            {
                if (index >= n) break;
                StartCoroutine(ActiveCoroutine(go, currentScores[index]));
                index++;
            }
        }

        IEnumerator ActiveCoroutine(Transform go, int score)
        {
            int winningScore = CoreModel.winningScore;
            for (int i = 0; i < winningScore; i++)
            {
                string path = "Score/";
                GameObject _g = Instantiate(Resources.Load<GameObject>(path + nameOfScoreDisplay));
                _g.transform.SetParent(go);
                _g.transform.localScale = new Vector3(1, 1, 1);
                yield return null;
            }
            yield return null;
            go.gameObject.SetActive(true);
            view.Init(go.gameObject, score);
        }

        public IEnumerator StartScoring()
        {
            List<int> test = new List<int>();
            test.Add(3);
            model.ObtainScores = test;
            yield return StartCoroutine(ScoringCoroutine(model.ObtainScores));
        }

        public void RegisterGoalPlayerCallback(int id)
        {
            goalPlayers.Add(CoreModel.RoleAvatars[id]);
        }

        public void GameOver()
        {
            switch (goalPlayers.Count)
            {
                case 0:
                    changeGameStateAction("NewGame");
                    return;
                case 1:
                    CoreModel.WinnerAvatars = goalPlayers;
                    changeGameStateAction("GameOver");
                    break;
                default:
                    CoreModel.WinnerAvatars = goalPlayers;
                    changeGameStateAction("GameDraw");
                    break;
            }
        }

        IEnumerator ScoringCoroutine(List<int> scores)
        {
            List<int> currentScores = CoreModel.TotalScores;
            int index = 0;

            yield return new WaitForSecondsRealtime(1.5f);

            foreach (int s in scores)
            {
                var count = StartCoroutine(
                    view.SetUIToScore(
                        scoreObject.GetChild(index).gameObject,
                        currentScores[index],
                        currentScores[index] + s,
                        RegisterGoalPlayerCallback,
                        index
                    )
                );
                yield return count;
                yield return new WaitForSecondsRealtime(0.1f);
                currentScores[index] += s;
                if (currentScores[index] >= CoreModel.winningScore) RegisterGoalPlayerCallback(index);
                Debug.Log("P" + (index + 1) + " score : " + currentScores[index]);
                index++;
            }

            CoreModel.TotalScores = currentScores;

            yield return new WaitForSeconds(3);

            GameOver();
        }

        private void Awake()
        {
            view = GetComponent<View>();
            DontDestroyOnLoad(this);
        }
    }
}