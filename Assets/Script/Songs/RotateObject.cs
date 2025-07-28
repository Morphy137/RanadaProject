using UnityEngine;

namespace Script.Songs
{
    /// <summary>
    /// Componente simple que rota continuamente un objeto alrededor del eje Z.
    /// Útil para crear efectos visuales de rotación constante en elementos de la interfaz
    /// o decorativos que acompañen el ritmo musical.
    /// </summary>
    public class RotateObject : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Configuración de Rotación")]
        [Tooltip("Velocidad de rotación en grados por segundo")]
        public float rotationSpeed = 100f;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Aplica rotación continua al objeto alrededor del eje Z.
        /// La rotación es independiente del framerate gracias al uso de Time.deltaTime.
        /// </summary>
        void Update()
        {
            // Rotate the object around the Z axis
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        #endregion
    }
}
