using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerSpace.Game
{
    public class Fields : MonoBehaviour
    {
        float speedGain;
        float gravity;

        Model model = null;
        Rigidbody2D rb = null;
        private void Start()
        {
            model = GetComponent<Model>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Fields") {
                string[] handler = other.name.Split(',');
                string name = handler[0];
                switch (name)
                {
                    case "Boost":
                        speedGain = float.Parse(handler[1]);
                        model.AddItemSpeedGain = speedGain;
                        model.AddItemJumpGain = speedGain;
                        break;
                    case "Floating":
                        
                        break;
                    case "Gravity":
                        gravity = float.Parse(handler[1]);
                        rb.gravityScale = gravity;
                        break;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Fields")
            {
                string[] handler = other.name.Split(',');
                string name = handler[0];
                switch (name)
                {
                    case "Boost":
                        speedGain = float.Parse(handler[1]);
                        model.AddItemSpeedGain = -speedGain;
                        model.AddItemJumpGain = -speedGain;
                        break;
                    case "Floating":

                        break;
                    case "Gravity":
                        rb.gravityScale = 9.8f;
                        break;
                }
            }
        }
    }
}

