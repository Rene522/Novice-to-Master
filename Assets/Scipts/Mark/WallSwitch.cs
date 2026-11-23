using UnityEngine;

public class WallSwitch : MonoBehaviour
{
    [SerializeField] Transform socketPoint;
    [SerializeField] float pushSpeed = 1f;

    bool isPushed;

    public void Push()
    {
        isPushed = true;
    }

    private void Update()
    {
        if (!isPushed)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, socketPoint.position, pushSpeed * Time.deltaTime);

        if (transform.position == socketPoint.position)
        {
            isPushed = false;
        }
    }
}