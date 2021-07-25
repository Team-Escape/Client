using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace.Gameplayer
{
    public class View : MonoBehaviour
    {
        [SerializeField] UIHint uiHint;
        [SerializeField] HealthBar healthBar;

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
            uiHint.image.sprite = isKeyboard ? uiHint.keyboardSprite : uiHint.joystickSprite;
        }
        #endregion

        public void UpdateShaderRender(string effect)
        {
            Material material = GetComponent<Renderer>().material;
            material.EnableKeyword(effect);
        }

        public void UpdateHintUI(bool isActive)
        {
            uiHint.image.gameObject.SetActive(isActive);
        }
        public void UpdateHintUI(bool isActive, Transform pos)
        {
            uiHint.image.gameObject.SetActive(isActive);
        }

        public void UpdateHealthbar(float amount)
        {
            healthBar.health.fillAmount = amount;
        }
        public void UpdateEndurancebar(float amount)
        {
            healthBar.endurance.fillAmount = amount;
        }
    }

    [System.Serializable]
    public class UIHint
    {
        public Image image;
        public Sprite keyboardSprite;
        public Sprite joystickSprite;
    }

    [System.Serializable]
    public class HealthBar
    {
        public Image health;
        public Image endurance;
    }
}