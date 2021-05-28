using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollows : MonoBehaviour
{
    public GameObject player;
    void Update() {
        this.transform.position = player.transform.position;
    }
}
