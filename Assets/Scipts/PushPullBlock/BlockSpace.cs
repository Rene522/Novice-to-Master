using UnityEngine;

/// <summary>
/// This class represents a block space that can hold a specific block. It checks whether the correct block is placed in the space within a specified tolerance.
/// </summary>
public class BlockSpace : MonoBehaviour
{
    [Tooltip("The correct block that should be placed in this space.")]
    [SerializeField] PushPullBlock correctBlock;
    [Tooltip("The tolerance for placing a block in this space.")]
    [SerializeField] float placementTolerance = 0.1f;

    /// <summary>
    /// Determines whether the correct block is currently placed in this block space within the specified tolerance.
    /// </summary>
    /// <returns></returns>
    public bool IsFilled()
    {
        if (!correctBlock || correctBlock.IsMoving()) // If the correct block is not assigned or is currently moving, the space is not considered filled
        {
            return false;
        }
        Vector3 distanceToBlock = correctBlock.transform.position - transform.position; // Calculate the distance from the correct block to this block space
        distanceToBlock.y = 0f; // Ignore vertical distance to only consider horizontal placement
        return distanceToBlock.magnitude <= placementTolerance; // Return true if the distance is within the placement tolerance, indicating the block is correctly placed
    }
}
