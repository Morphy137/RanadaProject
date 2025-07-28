using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Script.Animation
{
    /// <summary>
    /// Controla la animación de entrada y salida del indicador de combo cuando el jugador alcanza 5 hits consecutivos.
    /// </summary>
    public class ComboIEnumerator : MonoBehaviour
    {
        #region Constants
        private const float AnimationSpeed = 15f;
        #endregion

        #region Private Fields
        private Coroutine currentAnimation;
        private Vector3 initialPosition;
        private Vector3 targetPosition;
        private bool isComboActive;
        private bool isMoving;
        private Image image;
        private TextMeshProUGUI textMeshPro;
        private Color colorText;
        #endregion

        #region Serialized Fields
        [SerializeField] private GameObject comboSprite;
        [SerializeField] private GameObject comboText;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa las posiciones y componentes necesarios para la animación.
        /// </summary>
        private void Start()
        {
            initialPosition = transform.localPosition;
            targetPosition = initialPosition - new Vector3(0, 175, 0);
            isComboActive = false;
            isMoving = false;
            
            image = comboSprite.GetComponent<Image>();
            textMeshPro = comboText.GetComponent<TextMeshProUGUI>();
            colorText = textMeshPro.color;
        }

        /// <summary>
        /// Verifica el estado del combo y activa/desactiva la animación según corresponda.
        /// </summary>
        private void Update()
        {
            if (GlobalScore.currentCombo >= 5 && !isComboActive)
            {
                StartCombo();
                isComboActive = true;
            }
            else if(GlobalScore.currentCombo < 5 && isComboActive)
            {
                BreakCombo();
                isComboActive = false;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inicia la animación de entrada del indicador de combo.
        /// </summary>
        public void StartCombo()
        {
            if (isMoving && currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(MoveToTarget(targetPosition, true));
        }

        /// <summary>
        /// Inicia la animación de salida del indicador de combo.
        /// </summary>
        public void BreakCombo()
        {
            if (isMoving && currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(MoveToTarget(initialPosition, false));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Anima el movimiento del combo hacia la posición objetivo con efecto de transparencia.
        /// </summary>
        /// <param name="target">Posición objetivo del movimiento</param>
        /// <param name="isComboActive">Si el combo está activo o no</param>
        private IEnumerator MoveToTarget(Vector3 target, bool isComboActive)
        {
            isMoving = true;
            float distance = Vector3.Distance(transform.localPosition, target);
            float startTime = Time.time;
            
            image = comboSprite.GetComponent<Image>();
            textMeshPro = comboText.GetComponent<TextMeshProUGUI>();
            
            while (Vector3.Distance(transform.localPosition, target) > 0.01f)
            {
                float t = (Time.time - startTime) * AnimationSpeed / distance;
                transform.localPosition = Vector3.Lerp(transform.localPosition, target, t);
                
                Color color = image.color;
                Color colorText = textMeshPro.color;
                
                // Calcula la transparencia basada en la distancia
                color.a = Vector3.Distance(transform.localPosition, initialPosition) / distance;
                colorText.a = color.a;

                textMeshPro.color = colorText;
                image.color = color;
                yield return new WaitForFixedUpdate();
            }
            
            transform.localPosition = target;
            isMoving = false;
        }
        #endregion
    }
}