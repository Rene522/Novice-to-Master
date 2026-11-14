using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector3 ClosePosition;
    public Vector3 OpenPosition;

    public bool isClosed = true;
    public bool MultipleRequirements;
    public List<PuzzleItem> items = new List<PuzzleItem>();

    private void Update()
    {
        if (MultipleRequirements)
        {
            CheckObjects();
        }

        if (isClosed)
        {
            MoveToPos(ClosePosition);
        }
        else
        {
            MoveToPos(OpenPosition);
        }
    }

    public void MoveToPos(Vector3 pos)
    {
        if ((this.transform.localPosition - pos).magnitude > 0.01f)
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, pos, 0.2f);
        }
        else
        {
            this.transform.localPosition = pos;
        }
    }

    public void CheckObjects()
    {
        bool allComplete = true;

        foreach (PuzzleItem item in items)
        {
            if (!item.isSolved)
            {
                allComplete = false;
            }
        }

        if (allComplete)
        {
            isClosed = false;
        }
        else
        {
            isClosed = true;
        }
    }
}
