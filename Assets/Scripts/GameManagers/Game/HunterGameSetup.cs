using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace.HunterGame;
using Rewired;

namespace GameManagerSpace.Game.HunterGame
{
    public class HunterGameSetup : MonoBehaviour
    {
        [SerializeField] Camera cam = null;
        [SerializeField] GameObject dot = null;
        [SerializeField] RectTransform canvas = null;
        [SerializeField] RectTransform parentObject = null;
        [SerializeField] HunterGamePlayer hunterGamePlayer = null;

        private void Awake()
        {
            canvas.GetComponent<Canvas>().worldCamera = cam;
        }

        public void Generator(Player player, System.Action callback)
        {
            GeneratorCoroutine(player, callback);
        }

        void GeneratorCoroutine(Player player, System.Action callback)
        {
            float sizeX = 100 * cam.rect.width;
            float sizeY = sizeX;// 100 * cam.rect.height;

            float width = 1920 / 2 * cam.rect.width;
            float height = 1080 / 2 * cam.rect.height;
            Vector2 screenSize = new Vector2(width, height);
            Vector2 pos = new Vector2(width, height + sizeY);
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Vector2.zero, cam, out pos);


            // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
            // ███  Change the priority of generation  █████████████████████████████████████████████████████████████████████████████
            // ███  Down -> Top -> Right -> Down -> Left -> ....  ██████████████████████████████████████████████████████████████████
            // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
            while (pos.y >= -height - sizeY)
            {
                while (pos.x < width)
                {
                    GameObject go = Instantiate(dot);
                    go.transform.name = "Dot";
                    go.layer = LayerMask.NameToLayer("HunterGame");
                    go.GetComponent<RectTransform>().SetParent(parentObject);
                    go.GetComponent<BoxCollider2D>().size = new Vector2(sizeX, sizeY);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
                    go.GetComponent<RectTransform>().localPosition = pos;
                    go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    pos += new Vector2(sizeX, 0);
                }
                pos = new Vector2(-width, pos.y);
                pos += new Vector2(0, -sizeY);
            }

            if (callback != null) callback();
            ActivePlayer(player, screenSize, new Vector2(sizeX, sizeY));
        }

        void ActivePlayer(Player player, Vector2 screenSize, Vector2 size)
        {
            hunterGamePlayer.gameObject.SetActive(true);
            hunterGamePlayer.Init(player, screenSize, size);
        }
    }

}