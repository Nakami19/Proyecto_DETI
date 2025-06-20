using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 moveDirection = Vector3.up; // e.g., (0,1,0) for vertical, (1,0,0) for horizontal
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public float waitTime = 1f;
    public bool useLocalPosition = true;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingForward = true;
    private bool isWaiting = false;

    void Start()
    {
        startPosition = useLocalPosition ? transform.localPosition : transform.position;
        targetPosition = startPosition + moveDirection.normalized * moveDistance;
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            if (isWaiting)
            {
                yield return null;
                continue;
            }

            Vector3 currentPosition = useLocalPosition ? transform.localPosition : transform.position;
            Vector3 destination = movingForward ? targetPosition : startPosition;

            // Move toward the destination
            currentPosition = Vector3.MoveTowards(currentPosition, destination, moveSpeed * Time.deltaTime);

            if (useLocalPosition)
                transform.localPosition = currentPosition;
            else
                transform.position = currentPosition;

            // Check if reached
            if (Vector3.Distance(currentPosition, destination) < 0.01f)
            {
                isWaiting = true;
                yield return new WaitForSeconds(waitTime);
                movingForward = !movingForward;
                isWaiting = false;
            }

            yield return null;
        }
    }
}
