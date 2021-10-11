using UnityEngine;
using GameManagerSpace.Game;
public class GMControlTest : MonoBehaviour {
    Control control;
    GameManager gameManager;
    private void Awake() {
        control = GetComponent<Control>();
        control.Init(
            (string name) => gameManager.GameFlow(name)
        );
    }
    private void Start()
        {
            gameManager.GameFlow("Setting");
        }
}