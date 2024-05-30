using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Rotational speed in degrees per second
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the object around the Z axis
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
