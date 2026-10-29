using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject follow;
    public GameObject visual;

    private void Start()
    {
        if (follow == null)
        {
            CharacterController temp = FindAnyObjectByType<CharacterController>();

            if (temp != null)
            {
                follow = temp.gameObject;
            }
        }
    }

    private void Update()
    {
        if ((this.transform.position - follow.transform.position).magnitude > 10)
        { 
            visual.gameObject.SetActive(false);
        }
        else
        {
            visual.gameObject.SetActive(true);
        }

        transform.LookAt(follow.transform);
    }
}
