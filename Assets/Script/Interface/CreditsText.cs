using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public RectTransform creditsText; // Asigna aquí el RectTransform del texto de créditos.
    public RectTransform logoImage;   // Asigna aquí el RectTransform del logo del juego.
    public float speed = 50f;         // Velocidad de desplazamiento de los créditos.

    private Vector3 initialPositionText; // Posición inicial del texto.
    private Vector3 initialPositionLogo; // Posición inicial del logo.
    private bool isCreditsRolling = false; // Controla si los créditos se están moviendo.

    void Awake()
    {
        // Guardar las posiciones iniciales del texto y el logo.
        initialPositionText = creditsText.localPosition;
        initialPositionLogo = logoImage.localPosition;
        Debug.Log("Posición inicial del texto: " + initialPositionText);
    }

    void Update()
    {
        if (isCreditsRolling)
        {
            // Mover el texto y el logo hacia arriba con la velocidad definida.
            creditsText.localPosition += Vector3.up * speed * Time.deltaTime;
            logoImage.localPosition += Vector3.up * speed * Time.deltaTime;
        }
    }

    public void ShowCredits()
    {
        // Restablecer las posiciones iniciales del texto y el logo antes de iniciar los créditos.
        creditsText.localPosition = initialPositionText; // Ajustar la posición inicial del texto más abajo.
        logoImage.localPosition = initialPositionLogo; // Posición inicial del logo.

        // Iniciar el movimiento de los créditos.
        isCreditsRolling = true;
    }

    public void StopCredits()
    {
        // Detener el movimiento de los créditos.
        isCreditsRolling = false;
    }
}
