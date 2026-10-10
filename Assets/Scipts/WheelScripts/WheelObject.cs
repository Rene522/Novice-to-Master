using Unity.VisualScripting;
using UnityEngine;

public class WheelObject : MonoBehaviour
{
    public int RelativePosition;

    public Vector3 desiredPos;
    public bool isCenter;

    public void Update()
    {
        if (isCenter)
        {
            if ((this.transform.localPosition - new Vector3(0,0,0)).magnitude > 0.1f)
            {
                GoTo(new Vector3(0, 0, 0));
            }
            else
            {
                this.transform.localPosition = new Vector3(0, 0, 0);
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
    }
    public void GoTo(Vector3 newPos)
    {
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, newPos, 0.1f);
    }
}
