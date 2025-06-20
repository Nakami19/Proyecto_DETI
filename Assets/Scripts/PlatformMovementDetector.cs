using UnityEngine;

public class PlatformMovementDetector : MonoBehaviour
{
    public Vector3 LastPosition { get; private set; }
    public Quaternion LastRotation { get; private set; }

    public Vector3 DeltaPosition { get; private set; }
    public Quaternion DeltaRotation { get; private set; }

    void Start()
    {
        LastPosition = transform.position;
        LastRotation = transform.rotation;
    }

    void LateUpdate()
    {
        DeltaPosition = transform.position - LastPosition;
        DeltaRotation = transform.rotation * Quaternion.Inverse(LastRotation);

        LastPosition = transform.position;
        LastRotation = transform.rotation;
    }
}
