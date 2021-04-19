using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameManagerSpace.Menu
{
    public class View : MonoBehaviour, IMenuView
    {
        /// <summary>
        /// 1.<!--Setup button pressed feedback.-->
        /// 2.<!--Attatch ButtonHighLight to button and assign onClick function to button.-->
        /// </summary>
        /// <param name="ButtonHighLight">
        /// The 'script' attatchs to button which is used to dectect select or deselect
        /// </param>
        public Button CurrentHighLightButton { get; set; }
        [SerializeField] GameObject canvas = null;
        [SerializeField] List<Button> buttons = new List<Button>();
        List<ButtonHighlighted> buttonHighlighteds = new List<ButtonHighlighted>();

        public void NextButton()
        {
            for (int i = 0; i < buttons.Count - 1; i++)
            {
                if (CurrentHighLightButton == buttons[i])
                {
                    buttonHighlighteds[i].Deselected();
                    buttonHighlighteds[i + 1].Selected();
                    CurrentHighLightButton = buttons[i + 1];
                    break;
                }
            }
        }
        public void PrevButton()
        {
            for (int i = buttons.Count - 1; i > 0; i--)
            {
                if (CurrentHighLightButton == buttons[i])
                {
                    buttonHighlighteds[i].Deselected();
                    buttonHighlighteds[i - 1].Selected();
                    CurrentHighLightButton = buttons[i - 1];
                    break;
                }
            }
        }

        public void Click()
        {
            CurrentHighLightButton.GetComponent<ButtonHighlighted>().Pressed();
        }

        public void HighlighCallback(Button button)
        {
            if (CurrentHighLightButton == button) return;
            CurrentHighLightButton.GetComponent<ButtonHighlighted>().Deselected();
            button.GetComponent<ButtonHighlighted>().Selected();
            CurrentHighLightButton = button;
        }

        public void InitButton()
        {
            foreach (Button btn in buttons)
            {
                btn.gameObject.transform.SetParent(canvas.transform);

                ButtonHighlighted buttonController = btn.GetComponent<ButtonHighlighted>();

                buttonHighlighteds.Add(buttonController);
                buttonController.Init(HighlighCallback);
            }
            CurrentHighLightButton = buttons[0];
            buttonHighlighteds[0].Selected();
        }

    }
}