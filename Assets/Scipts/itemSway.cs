using UnityEngine.InputSystem;
using UnityEngine;
public class itemSway : MonoBehaviour
{
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Get the mouse delta movement
        Vector2 mouse = Mouse.current.delta.ReadValue();

        //Control how strong the sway is
        float mouseX = mouse.x * swayMultiplier;
        float mouseY = mouse.y * swayMultiplier;

        //Create rotation based on how the mouse moves
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        //Combine both rotations to work together
        Quaternion targetRotation = rotationX * rotationY;

        //Controls how smooth the sway is
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

}