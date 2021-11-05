using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace GameManagerSpace.Game
{
    public class View : MonoBehaviour
    {
        
        [SerializeField]TMP_Text GoalCount;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void StartCount(){
            GoalCount.gameObject.SetActive(true);
        }
        public void CountDown(float timecount){
            GoalCount.text=((int)timecount).ToString();
        }
        public void EndCount(){
            GoalCount.text = "";
            GoalCount.gameObject.SetActive(false);
        }
    }
}