using UnityEngine;

namespace Script.Interface
{
    /// <summary>
    /// Implementa el patrón singleton para objetos que deben persistir entre cambios de escena.
    /// Previene la destrucción del GameObject al cargar nuevas escenas.
    /// </summary>
    public class DontDestroyObject : MonoBehaviour
    {
        #region Private Fields
        private static DontDestroyObject instance;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Implementa el patrón singleton y marca el objeto para no ser destruido.
        /// Si ya existe una instancia, destruye este GameObject duplicado.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
}