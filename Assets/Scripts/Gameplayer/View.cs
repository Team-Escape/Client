using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace PlayerSpace.Gameplayer
{
    public class View : MonoBehaviour
    {
        [SerializeField] UIHint uiHint;
        [SerializeField] HealthBar healthBar;
        [SerializeField] Image gameItemUI;
        [SerializeField] SpriteRenderer startItemUI;
        #region Player Number Display        
        [SerializeField] List<Sprite> playerHintSprite = null;
        [SerializeField] Image playerHintImage = null;
        #endregion
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
        public void Init(bool isKeyboard, int id)
        {
            uiHint.image.sprite = isKeyboard ? uiHint.keyboardSprite : uiHint.joystickSprite;
            playerHintImage.sprite = playerHintSprite[id];
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
        public void UpdateStartItemUI(Sprite sprite)
        {
            if (sprite == null)
            {
                startItemUI.gameObject.SetActive(false);
                startItemUI.sprite = null;
            }
            else
            {
                startItemUI.sprite = sprite;
                startItemUI.gameObject.SetActive(true);
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