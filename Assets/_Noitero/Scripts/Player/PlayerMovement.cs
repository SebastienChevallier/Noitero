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
        if (joystick == null || _characterController == null)
            return;

        _characterController.Move(GetJoystickDirection() * speed * Time.deltaTime);
    }

    public Vector3 GetJoystickDirection() =>
        new Vector3(joystick.Horizontal, 0f, joystick.Vertical).normalized;
}
