using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Script.Notes;

public class PauseUntilKeyPress : MonoBehaviour
{
    public Canvas tutorialCanvas; // El canvas que contiene el mensaje "Presiona Para Continuar"
    public Canvas ScoreUI;
    public Canvas PauseButton;
    private bool isStart;

    void Start()
    {
        isStart = false;
        ScoreUI.enabled = false;
        PauseButton.enabled = false;
    }

    void Update()
    {
        // Detectar si se presiona cualquier tecla
        if (isStart == false) {
            if (Input.anyKeyDown)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {

        // Ocultar el Canvas cuando se reanuda el juego
        isStart = true;
        tutorialCanvas.enabled = false;
        ScoreUI.enabled = true;
        PauseButton.enabled = true;
        SongManager.Instance.InitSong();
    }
}
