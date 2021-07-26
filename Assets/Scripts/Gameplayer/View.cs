using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace.Gameplayer
{
    public class View : MonoBehaviour
    {
        [SerializeField] UIHint uiHint;
        [SerializeField] HealthBar healthBar;
        [SerializeField] Image gameItemUI;

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

        public void UpdateGameItemUI(Sprite sprite)
        {
            if (sprite == null)
            {
                gameItemUI.gameObject.SetActive(false);
                gameItemUI.sprite = null;
            }
            else
            {
                gameItemUI.sprite = sprite;
                gameItemUI.gameObject.SetActive(true);
            }
        }
        public void UpdateShaderRender(string effect)
        {
            Material material = GetComponent<Renderer>().material;
            material.EnableKeyword(effect);
        }
        public void UpdateHintUI(bool isActive)
        {
            if (uiHint.image == null) return;
            uiHint.image.gameObject.SetActive(isActive);
        }
        public void UpdateHintUI(bool isActive, Vector2 pos)
        {
            if (uiHint.image == null) return;
            uiHint.image.transform.position = pos;
            uiHint.image.gameObject.SetActive(isActive);
        }
        public void UpdateHealthbar(float amount)
        {
            if (healthBar.health == null) return;
            healthBar.health.fillAmount = amount;
        }
        public void UpdateEndurancebar(float amount)
        {
            if (healthBar.endurance == null) return;
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