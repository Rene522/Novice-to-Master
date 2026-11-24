using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] BlockSpace blockSpace;
    [SerializeField] float moveSpeed = 2f;

    Vector3 closedPosition;
    Vector3 openPosition;

    void Start()
    {
        closedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        MeshRenderer doorRenderer = GetComponent<MeshRenderer>();
        float doorWidth = doorRenderer.localBounds.size.z;

        Vector3 movement = transform.TransformVector(new Vector3(0f, 0f, doorWidth));

        openPosition = new Vector3(closedPosition.x + movement.x, closedPosition.y + movement.y, closedPosition.z + movement.z);
    }

    void Update()
    {
        if (!blockSpace)
        {
            return;
        }

        Vector3 targetPosition = closedPosition;

        if (blockSpace.IsFilled())
        {
            targetPosition = openPosition;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
