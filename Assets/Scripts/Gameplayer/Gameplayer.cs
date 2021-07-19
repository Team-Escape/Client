using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace PlayerSpace.Gameplayer
{
    public class Gameplayer : MonoBehaviour
    {
        [SerializeField] bool testMode = false;
        Player input = null;
        Control control = null;

        public int playerID = 0;
        public int teamID = 0;

        public void AssignController(int id)
        {
            playerID = id;
            input = ReInput.players.GetPlayer(playerID);
        }

        private void Awake()
        {
            if (testMode) AssignController(0);
            control = GetComponent<Control>();
        }

        private void MoveHandler()
        {
            if (input.GetButton("MoveR"))
            {
                control.Move(1);
            }
            else if (input.GetButton("MoveL"))
            {
                control.Move(-1);
            }
            else if (input.GetButtonUp("MoveL") || input.GetButtonUp("MoveR"))
            {
                control.Move(0);
            }

            if (input.GetButton("Jump"))
            {
                control.Jump(true);
            }
            else if (input.GetButtonUp("Jump"))
            {
                control.Jump(false);
            }
        }

        private void Update()
        {
            MoveHandler();
        }

        #region OnTriggerFuncs
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "DashItem":
                    string[] nameSplice = other.name.Split(',');
                    float forceX, forceY = 0;
                    forceX = (string.Compare("n", nameSplice[0]) == 0) ? transform.lossyScale.x : float.Parse(nameSplice[0]);
                    forceY = (string.Compare("n", nameSplice[1]) == 0) ? transform.lossyScale.y : float.Parse(nameSplice[1]);
                    control.DoDash(new Vector2(forceX, forceY));
                    break;
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {

        }
        private void OnTriggerExit2D(Collider2D other)
        {

        }
        #endregion
    }
}