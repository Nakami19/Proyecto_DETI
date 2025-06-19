using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 45f; // Degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
