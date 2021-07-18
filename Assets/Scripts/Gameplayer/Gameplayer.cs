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