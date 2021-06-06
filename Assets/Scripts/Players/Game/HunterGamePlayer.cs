using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace PlayerSpace.HunterGame
{
    public class HunterGamePlayer : MonoBehaviour
    {
        [SerializeField] int targetScore = 180;
        [SerializeField] float speed = 5;

        private Player player = null;
        private Vector3 moveVector = Vector3.zero;
        private Camera selfCamera = null;
        RectTransform selfTransform = null;
        private int score = 0;
        private string targetName = "Dot";
        private List<float> border = new List<float>();

        public void ExitHunterGame()
        {
            ChangeInputMap("GamePlay");

            this.gameObject.SetActive(false);
        }

        public void Init(Player p, Vector2 screeSize, Vector2 size)
        {
            player = p;
            GetComponent<RectTransform>().sizeDelta = size;
            GetComponent<CircleCollider2D>().radius = size.x / 2;
            ChangeInputMap("HunterGame");
            border.Add(screeSize.x * 1.25f);
            border.Add(screeSize.y * 1.25f);
            border.Add(-(screeSize.x * 1.25f));
            border.Add(-(screeSize.y * 1.25f));
            foreach (var a in border)
            {
                Debug.Log(a);
            }

        }

        void ChangeInputMap(string map)
        {
            player.controllers.maps.SetAllMapsEnabled(false);
            player.controllers.maps.SetMapsEnabled(true, map);
        }

        private void Update()
        {
            if (player != null)
            {
                Move();
                if (player.GetButtonDown("ExitHunterGame"))
                    ExitHunterGame();
            }
        }

        void Move()
        {
            moveVector.x = player.GetAxis("H-MoveX") * speed;
            moveVector.y = player.GetAxis("H-MoveY") * speed;

            selfTransform.position += moveVector;

            if (selfTransform.localPosition.x > border[0] || selfTransform.localPosition.x < border[2] || selfTransform.localPosition.y > border[1] || selfTransform.localPosition.y < border[3])
                selfTransform.localPosition = new Vector3(0, 0, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == targetName)
            {
                other.gameObject.SetActive(false);
                score++;
                if (score / 2 >= targetScore) ExitHunterGame();
            }
        }
        private void Awake()
        {
            score = 0;
            selfTransform = GetComponent<RectTransform>();
            selfCamera = transform.parent.GetComponentInChildren<Camera>();
        }

        private void OnDisable()
        {
            ChangeInputMap("GamePlay");
        }
    }
}