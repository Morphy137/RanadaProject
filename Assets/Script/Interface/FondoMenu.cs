using UnityEngine;
using UnityEngine.UI;

namespace Script.Interface
{
    /// <summary>
    /// Controla el movimiento automático de las imágenes de fondo del menú.
    /// Crea un efecto de desplazamiento continuo modificando las coordenadas UV.
    /// </summary>
    public class FondoMenu : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private RawImage[] fondos;
        [SerializeField] private float horizontal;
        [SerializeField] private float vertical;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Actualiza continuamente la posición UV de cada imagen de fondo para crear movimiento.
        /// </summary>
        private void Update()
        {
            foreach (var fondo in fondos)
            {
                fondo.uvRect = new Rect(fondo.uvRect.position + new Vector2(horizontal, vertical), fondo.uvRect.size);
            }    
        }
        #endregion
    }
}
