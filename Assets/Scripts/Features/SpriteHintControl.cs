using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace.Gameplayer;
using GameManagerSpace;

public class SpriteHintControl : MonoBehaviour
{
    public Sprite keyboard = null;
    public Sprite joystick = null;
    List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("123qwfwq[opmf[woq");
        if (other.tag == "Player")
        {
            Gameplayer role = other.gameObject.GetComponentInChildren<Gameplayer>();
            int mode = CoreModel.ActivePlayers[role.playerID].controllers.joystickCount > 0 ? 0 : 1;
            sprites[role.playerID].sprite = mode == 0 ? joystick : keyboard;
            sprites[role.playerID].gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Gameplayer role = other.gameObject.GetComponentInChildren<Gameplayer>();
            sprites[role.playerID].gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        foreach (Transform t in transform)
        {
            sprites.Add(t.GetComponent<SpriteRenderer>());
        }
        Debug.Log(sprites.Count);
    }
}
