using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace GameManagerSpace.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        // Responsible for playing animation only, not the UIs that can be interactived.
        public GameObject animationUI;
        public GameObject buttonUI;
        [SerializeField] string hallScene = "HallScene";
        [SerializeField] string settingsScene = "SettingsScene";
        System.Action<string, bool> loadSceneAction;
        System.Action audioAction;

        View view = null;
        bool ableToClickButton = false;

        public void Init(System.Action<string, bool> callback, System.Action audioCallback)
        {
            loadSceneAction = callback;
            audioAction = audioCallback;
        }

        public void AnimationEventCallback()
        {
            TimelineController afkTimeline = FindObjectOfType<TimelineController>();
            afkTimeline.Init();
            SwitchUICanvas();
            view.InitButton();
            ableToClickButton = true;
            audioAction();
        }

        void SwitchUICanvas()
        {
            animationUI.SetActive(false);
            buttonUI.SetActive(true);
        }

        public void Play()
        {
            if (ableToClickButton == false) return;
            loadSceneAction(hallScene, false);
        }

        public void Settings()
        {
            if (ableToClickButton == false) return;
            loadSceneAction(settingsScene, false);
        }

        public void Exit()
        {
            if (ableToClickButton == false) return;
            Application.Quit();
        }

        public void Confirm()
        {
            if (ableToClickButton == false) return;
            if (ReInput.players.GetSystemPlayer().GetButtonDown("Next"))
            {
                view.NextButton();
            }
            if (ReInput.players.GetSystemPlayer().GetButtonDown("Prev"))
            {
                view.PrevButton();
            }
            if (ReInput.players.GetSystemPlayer().GetButtonDown("Confirm"))
            {
                view.Click();
                // view.CurrentHighLightButton.onClick.Invoke();
            }
        }

        /// <summary>
        /// Menu UI Handling
        /// </summary>
        private void Awake()
        {
            view = GetComponent<View>();
        }
        private void Update()
        {
            Confirm();
        }
    }
}