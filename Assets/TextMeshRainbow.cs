using UnityEngine;
using TMPro; // Asegúrate de importar TextMeshPro

public class RainbowTextMeshPro : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; // Cambiado a privado para asignarlo automáticamente
    public float speed = 1.0f; // Velocidad del cambio de color

    void Awake()
    {
        // Asignar automáticamente el componente TextMeshProUGUI si está en el mismo GameObject
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Si estás utilizando un TextMeshPro en 3D (no UI), puedes cambiar a:
        // textMeshPro = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        // Cambiar el color con un ciclo RGB
        float r = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f;
        float g = Mathf.Sin(Time.time * speed + Mathf.PI / 3) * 0.5f + 0.5f;
        float b = Mathf.Sin(Time.time * speed + 2 * Mathf.PI / 3) * 0.5f + 0.5f;

        // Asignar el color al TextMeshPro
        textMeshPro.color = new Color(r, g, b);
    }
}
