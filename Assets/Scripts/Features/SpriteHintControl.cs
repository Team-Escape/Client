using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace.Game;
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
            PlayerCharacter role = other.gameObject.GetComponentInChildren<PlayerCharacter>();
            int mode = CoreModel.ActivePlayers[role.playerId].controllers.joystickCount > 0 ? 0 : 1;
            sprites[role.playerId].sprite = mode == 0 ? joystick : keyboard;
            sprites[role.playerId].gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerCharacter role = other.gameObject.GetComponentInChildren<PlayerCharacter>();
            sprites[role.playerId].gameObject.SetActive(false);
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
