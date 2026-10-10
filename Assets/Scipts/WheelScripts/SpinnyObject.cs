using UnityEngine;

public class SpinnyObject : MonoBehaviour
{
    public void Spin(float input)
    {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, 0, input), 0.1f);
        // [0,0,Z] rotates objects relative to the camera (for 2d), to Z degrees
    }
}
