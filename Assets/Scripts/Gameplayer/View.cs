using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace.Gameplayer
{
    public class View : MonoBehaviour
    {
        [SerializeField] Image hintUI;
        [SerializeField] Sprite keyboardSprite;
        [SerializeField] Sprite joystickSprite;

        Animator anim;

        #region Unity APIs
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// Init stuff, decide controller type.
        /// </summary>
        /// <param name="isKeyboard"></param>
        public void Init(bool isKeyboard)
        {
            hintUI.sprite = isKeyboard ? keyboardSprite : joystickSprite;
        }
        #endregion

        public void UpdateShaderRender(string effect)
        {
            Material material = GetComponent<Renderer>().material;
            material.EnableKeyword(effect);
        }

        public void UpdateHintUI(bool isActive)
        {
            hintUI.gameObject.SetActive(isActive);
        }
        public void UpdateHintUI(bool isActive, Transform pos)
        {
            hintUI.gameObject.SetActive(isActive);
        }
    }
}