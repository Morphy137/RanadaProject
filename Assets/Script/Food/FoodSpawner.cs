using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Food
{
  public class FoodSpawner : MonoBehaviour
  { 
    [Header("Configuración de spawn de comida")]
    [SerializeField] private List<GameObject> foodPrefabs; // Lista de prefabs de comida
    [SerializeField] private Transform spawnPosition; // Posición de spawn
    [SerializeField] private float waitTime = 0.7f; // Tiempo de espera entre spawns

    [Header("Configuración de movimiento de comida")]
    [SerializeField] private float moveSpeed = 10f; // Velocidad de movimiento de la comida
    [SerializeField] private float despawnLimit = -25f; // Límite de despawn

    private void Start()
    {
      StartCoroutine(SpawnFood());
    }
    
    private IEnumerator SpawnFood()
    {
      while (true)
      {
        // Selecciona un prefab de comida de manera aleatoria
        GameObject foodPrefab = foodPrefabs[Random.Range(0, foodPrefabs.Count)];

        // Instancia la comida en la posición de spawn
        GameObject food = Instantiate(foodPrefab, spawnPosition.position, foodPrefab.transform.rotation);

        // Inicia una Coroutine para mover la comida y destruirla cuando alcance el límite de despawn
        StartCoroutine(MoveAndDestroyFood(food));

        // Espera un tiempo antes de spawnear la próxima comida
        yield return new WaitForSeconds(waitTime);
      }
    }

    private IEnumerator MoveAndDestroyFood(GameObject food)
    {
      // Mueve la comida hacia la izquierda hasta que alcanza el límite de despawn
      while (food.transform.position.x > despawnLimit)
      {
        food.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        yield return null;
      }

      // Destruye la comida
      Destroy(food);
    }
  }
}