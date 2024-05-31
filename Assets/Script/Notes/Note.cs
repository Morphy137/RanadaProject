using UnityEngine;
using System.Collections;

namespace Script.Notes
{
    public class Note : MonoBehaviour
    {
        double timeInstantiated;
        public float assignedTime;
        public bool movementEnabled = true;
        public GameObject placeholder; // PlaceHolder movement
        private bool isMoving = false;
        private Vector3 targetPosition = new Vector3(-10.87f, -2.17f, 0f); // Coordenadas exactas
        private Vector3 targetScale = new Vector3(0.50f, 0.50f, 0.50f); // Escala objetivo

        void Start()
        {
            timeInstantiated = SongManager.GetAudioSourceTime(); // Get the current time of the audio source
        }

        void Update()
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated; // Get the time since the note was instantiated
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

            if (t > 1)
            {
                Destroy(gameObject);
            }
            else if (!movementEnabled && !isMoving)
            {
                StartCoroutine(MoveAndScaleToPosition(targetPosition, targetScale, 0.2f));
            }
            else if (movementEnabled)
            {
                transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnY,
                    Vector3.right * SongManager.Instance.noteDespawnY, t);
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        private IEnumerator MoveAndScaleToPosition(Vector3 targetPosition, Vector3 targetScale, float duration)
        {
            isMoving = true;
            Vector3 startPosition = transform.position;
            Vector3 startScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition; // Ensure the final position is accurate
            transform.localScale = targetScale; // Ensure the final scale is accurate
            isMoving = false;
        }
    }
}
