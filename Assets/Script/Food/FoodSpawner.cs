using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Food
{
    /// <summary>
    /// Controla la generación automática de comida que se mueve por la pantalla.
    /// Spawna objetos de comida de manera aleatoria y los mueve hasta destruirlos.
    /// </summary>
    public class FoodSpawner : MonoBehaviour
    { 
        #region Serialized Fields
        [Header("Configuración de spawn de comida")]
        [SerializeField] private List<GameObject> foodPrefabs;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private float waitTime = 0.7f;

        [Header("Configuración de movimiento de comida")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float despawnLimit = -25f;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicia el proceso de spawning continuo de comida.
        /// </summary>
        private void Start()
        {
            StartCoroutine(SpawnFood());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Corrutina que spawna comida de manera infinita con intervalos de tiempo.
        /// Selecciona aleatoriamente un prefab de comida y lo instancia.
        /// </summary>
        private IEnumerator SpawnFood()
        {
            while (true)
            {
                GameObject foodPrefab = foodPrefabs[Random.Range(0, foodPrefabs.Count)];
                GameObject food = Instantiate(foodPrefab, spawnPosition.position, foodPrefab.transform.rotation);
                StartCoroutine(MoveAndDestroyFood(food));
                yield return new WaitForSeconds(waitTime);
            }
        }

        /// <summary>
        /// Mueve un objeto de comida hacia la izquierda y lo destruye al alcanzar el límite.
        /// </summary>
        /// <param name="food">Objeto de comida a mover</param>
        private IEnumerator MoveAndDestroyFood(GameObject food)
        {
            while (food.transform.position.x > despawnLimit)
            {
                food.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }

            Destroy(food);
        }
        #endregion
    }
}