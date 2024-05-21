using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Interface
{
  public class FondoMenu : MonoBehaviour
  {
    [SerializeField] private RawImage[] fondos;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    private void Update()
    {
      foreach (var fondo in fondos) // Itera sobre cada imagen de fondo
      {
        fondo.uvRect = new Rect(fondo.uvRect.position + new Vector2(horizontal, vertical), fondo.uvRect.size);
      }    
    }
  }
}
