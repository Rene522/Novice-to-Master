using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject follow;
    
    private void Update()
    {
        transform.LookAt(follow.transform);
    }
}
