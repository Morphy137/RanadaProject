using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Script.Animation
{
  public class ComboIEnumerator : MonoBehaviour
  {
    private const float AnimationSpeed = 15f; // Ajusta la velocidad de la animación
    private Coroutine currentAnimation;
    
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    
    private bool isComboActive;
    private bool isMoving;

    private Image image;
    private TextMeshProUGUI textMeshPro;

    private Color colorText;
    
    [SerializeField] private GameObject comboSprite;
    [SerializeField] private GameObject comboText;

    private void Start()
    {
      initialPosition = transform.localPosition;
      targetPosition = initialPosition - new Vector3(0, 175, 0); // Ajusta la dirección de la animación en 0,0,0
      isComboActive = false;
      isMoving = false;
      
      image = comboSprite.GetComponent<Image>();
      textMeshPro = comboText.GetComponent<TextMeshProUGUI>();
      colorText = textMeshPro.color;
    }

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

    public void StartCombo()
    {
      if (isMoving && currentAnimation != null)
      {
        StopCoroutine(currentAnimation);
      }
      currentAnimation = StartCoroutine(MoveToTarget(targetPosition, true));
    }

    public void BreakCombo()
    {
      if (isMoving && currentAnimation != null)
      {
        StopCoroutine(currentAnimation);
      }
      currentAnimation = StartCoroutine(MoveToTarget(initialPosition, false));
    }

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
        
        Color color = image.color; // Obtiene el color actual del material
        Color colorText = textMeshPro.color;
        
        // Calcula la transparencia en función de la distancia desde el punto de inicio
        color.a = Vector3.Distance(transform.localPosition, initialPosition) / distance;
        
        // Calcula la transparencia en función de la distancia desde el punto de inicio
        colorText.a = color.a;

        textMeshPro.color = colorText;// Ajusta la trasparencia del texto
        image.color = color; // Ajusta la trasparencia del sprite
        yield return new WaitForFixedUpdate();
      }
      transform.localPosition = target;
      isMoving = false;
    }
  }
}