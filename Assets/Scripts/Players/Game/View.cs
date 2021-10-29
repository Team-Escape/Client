using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace.Game
{
    [System.Serializable]
    public class RoleUIContainer
    {
        public GameObject GetPlayerCanvas { get { return playerCanvas; } }
        public Image HealthBar { get { return healthBar; } set { healthBar = value; } }
        public Image EnduranceBar { get { return enduranceBar; } set { enduranceBar = value; } }
        public Image Decoration { get { return decoration; } set { decoration = value; } }
        [SerializeField] GameObject playerCanvas = null;
        #region Player Number
        [SerializeField] Sprite playerIndex = null;
        [SerializeField] Image playerImage = null;
        #endregion
        [SerializeField] Image healthBar = null;
        [SerializeField] Image enduranceBar = null;
        [SerializeField] Image decoration = null;
    }
    public class View : MonoBehaviour
    {
        public RoleUIContainer selfView;

        public void UpdaetShaderRenderer(string effect)
        {
            Material mat = GetComponent<Renderer>().material;
            mat.EnableKeyword(effect);
        }

        public void UpdateHealthBar(float amount)
        {
            selfView.HealthBar.fillAmount = amount;
        }

        public void UpdateEnduranceBar(float amount)
        {
            selfView.EnduranceBar.fillAmount = amount;
        }

        public void UpdateDecoration(Image image)
        {
            selfView.Decoration = image;
            selfView.Decoration.enabled = true;
        }
    }
}