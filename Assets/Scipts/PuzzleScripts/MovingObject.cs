using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector3 ClosePosition;
    public Vector3 OpenPosition;

    public bool isClosed = true;

    private void Update()
    {
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
}
