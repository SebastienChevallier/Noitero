using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    public DynamicJoystick joystick;
    public float speed = 5f; // Speed of the player movement

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        if(joystick == null)
            return;

        if(_characterController == null)
        {
            Debug.LogError("CharacterController is not assigned or found on the GameObject.");
            return;
        }

        _characterController.Move(GetJoystickDirection() * Time.deltaTime * speed); // Adjust speed as needed
    }

    public Vector3 GetJoystickDirection()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        return direction.normalized;
    }
}
