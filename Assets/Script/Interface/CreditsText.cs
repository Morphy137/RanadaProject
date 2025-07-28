using UnityEngine;
using UnityEngine.UI;

namespace Script.Interface
{
    /// <summary>
    /// Controla la animación de desplazamiento de los créditos del juego.
    /// Gestiona el movimiento vertical automático del texto de créditos y el logo.
    /// </summary>
    public class CreditsController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Elementos de UI")]
        [Tooltip("RectTransform del texto de créditos que se desplazará")]
        public RectTransform creditsText;

        [Tooltip("RectTransform del logo del juego que acompañará el desplazamiento")]
        public RectTransform logoImage;

        [Header("Configuración de Animación")]
        [Tooltip("Velocidad de desplazamiento vertical en unidades por segundo")]
        public float speed = 50f;
        #endregion

        #region Private Fields
        /// <summary>Posición inicial del texto de créditos</summary>
        private Vector3 initialPositionText;

        /// <summary>Posición inicial del logo del juego</summary>
        private Vector3 initialPositionLogo;

        /// <summary>Indica si los créditos están actualmente en movimiento</summary>
        private bool isCreditsRolling = false;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa las posiciones originales del texto y logo al despertar el componente.
        /// </summary>
        void Awake()
        {
            // Guardar las posiciones iniciales del texto y el logo
            initialPositionText = creditsText.localPosition;
            initialPositionLogo = logoImage.localPosition;
            Debug.Log("Posición inicial del texto: " + initialPositionText);
        }

        /// <summary>
        /// Actualiza la posición de los elementos durante la animación de créditos.
        /// Se ejecuta cada frame cuando los créditos están en movimiento.
        /// </summary>
        void Update()
        {
            if (isCreditsRolling)
            {
                // Mover el texto y el logo hacia arriba con la velocidad definida
                creditsText.localPosition += Vector3.up * speed * Time.deltaTime;
                logoImage.localPosition += Vector3.up * speed * Time.deltaTime;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inicia la animación de los créditos desde sus posiciones iniciales.
        /// Restablece las posiciones y activa el movimiento automático.
        /// </summary>
        public void ShowCredits()
        {
            // Restablecer las posiciones iniciales del texto y el logo antes de iniciar los créditos
            creditsText.localPosition = initialPositionText;
            logoImage.localPosition = initialPositionLogo;

            // Iniciar el movimiento de los créditos
            isCreditsRolling = true;
        }

        /// <summary>
        /// Detiene inmediatamente la animación de los créditos.
        /// Los elementos permanecen en su posición actual.
        /// </summary>
        public void StopCredits()
        {
            // Detener el movimiento de los créditos
            isCreditsRolling = false;
        }
        #endregion
    }
}
