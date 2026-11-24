using Unity.VisualScripting;
using UnityEngine;

public class WheelObject : PuzzleItem
{
    public int RelativePosition;

    public Vector3 desiredPos;
    public Vector3 centerPos;
    public bool isCenter;


    public bool isSolvable;
    public int solvePos;

    public void Update()
    {
        if (isCenter)
        {
            if ((this.transform.localPosition - centerPos).magnitude > 0.1f)
            {
                GoTo(centerPos);
            }
            else
            {
                this.transform.localPosition = centerPos;
                isCenter = false;
            }
        }
        else if ((this.transform.localPosition - desiredPos).magnitude > 0.1f)
        {
            GoTo(desiredPos);
        }
        else
        { 
            this.transform.localPosition = desiredPos;
        }

        if (RelativePosition == solvePos)
        {
            Solve();
        }
    }
    public void GoTo(Vector3 newPos)
    {
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, newPos, 0.1f);
    }
}
