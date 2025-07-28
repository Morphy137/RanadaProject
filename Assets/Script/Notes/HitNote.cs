using UnityEngine;

namespace Script.Notes
{
  /// <summary>
  /// Componente simple para la destrucción controlada de notas musicales.
  /// Proporciona un método público para eliminar el GameObject cuando es necesario.
  /// </summary>
  public class HitNote : MonoBehaviour
  {
    #region Public Methods
    /// <summary>
    /// Destruye el GameObject actual. 
    /// Utilizado principalmente cuando una nota es golpeada exitosamente o necesita ser eliminada.
    /// </summary>
    public void DestroyGameObject()
    {
      Destroy(gameObject);
    }
    #endregion
  }
}