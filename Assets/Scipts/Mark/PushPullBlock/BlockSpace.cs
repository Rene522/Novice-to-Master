using System;
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
    [SerializeField] bool lockBlockInPlace;

    bool hasLocked;

    private void Update()
    {
        if (lockBlockInPlace && !hasLocked && IsFilled())
        {
            correctBlock.LockInPlace();
            hasLocked = true;
        }
    }

    /// <summary>
    /// Determines whether the correct block is currently placed in this block space within the specified tolerance.
    /// </summary>
    /// <returns></returns>
    public bool IsFilled()
    {
        if (!correctBlock || correctBlock.IsMoving())
        {
            return false;
        }

        Renderer blockRenderer = correctBlock.GetComponent<Renderer>();
        Renderer padRenderer = GetComponent<Renderer>();

        if (!blockRenderer || !padRenderer)
        {
            return false;
        }

        Bounds blockBounds = blockRenderer.localBounds;
        Bounds padBounds = padRenderer.localBounds;
        Vector3 scale = padRenderer.transform.lossyScale;

        if (Mathf.Abs(scale.x) < 0.0001f || Mathf.Abs(scale.z) < 0.0001f)
        {
            return false;
        }

        float toleranceX = placementTolerance / Mathf.Abs(scale.x);
        float toleranceZ = placementTolerance / Mathf.Abs(scale.z);

        for (int x = -1; x <= 1; x += 2)
        {
            for (int z = -1; z <= 1; z += 2)
            {
                Vector3 corner = new Vector3(blockBounds.center.x + blockBounds.extents.x * x, blockBounds.min.y, blockBounds.center.z + blockBounds.extents.z * z);

                corner = blockRenderer.transform.TransformPoint(corner);
                corner = padRenderer.transform.InverseTransformPoint(corner);

                if (corner.x < padBounds.min.x - toleranceX || corner.x > padBounds.max.x + toleranceX || corner.z < padBounds.min.z - toleranceZ || corner.z > padBounds.max.z + toleranceZ)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
