using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagerSpace.Menu
{
    interface IMenuView
    {
        void NextButton();
        void PrevButton();
        void Click();
        void HighlighCallback(Button button);
        void InitButton();
    }
}