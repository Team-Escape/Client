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
        private float[] border = {
            -105, 105, -55, 55
        };

        public void ExitHunterGame()
        {
            ChangeInputMap("GamePlay");

            this.gameObject.SetActive(false);
        }

        public void Init(Player p, Vector2 size)
        {
            player = p;
            GetComponent<RectTransform>().sizeDelta = size;
            GetComponent<CircleCollider2D>().radius = size.x / 2;
            ChangeInputMap("HunterGame");
        }

        void ChangeInputMap(string map)
        {
            player.controllers.maps.SetAllMapsEnabled(false);
            player.controllers.maps.SetMapsEnabled(true, map);
        }

        private void Update()
        {
            if (player != null)
                Move();
            if (player.GetButtonDown("ExitHunterGame"))
                ExitHunterGame();
        }

        void Move()
        {
            moveVector.x = player.GetAxis("H-Move Horizontal") * speed;
            moveVector.y = player.GetAxis("H-Move Vertical") * speed;

            if (selfTransform.position.x + moveVector.x > border[0] && selfTransform.position.x + moveVector.x < border[1])
            {
                selfTransform.position += new Vector3(moveVector.x, 0, 0);
            }
            else
            {
                selfTransform.position += new Vector3(-moveVector.x, 0, 0);
            }

            if (selfTransform.position.y + moveVector.y > border[2] && selfTransform.position.y + moveVector.y < border[3])
            {
                selfTransform.position += new Vector3(0, moveVector.y, 0);
            }
            else
            {
                selfTransform.position += new Vector3(0, -moveVector.y, 0);
            }
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
    }
}