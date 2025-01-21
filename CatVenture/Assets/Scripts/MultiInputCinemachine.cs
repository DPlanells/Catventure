using UnityEngine;
using Cinemachine;

public class MultiInputCinemachine : MonoBehaviour, AxisState.IInputAxisProvider
{
    // Sensitivity for inputs
    public float sensitivity = 1f;

    // Input Axis Names
    public string mouseXInput = "Mouse X";
    public string mouseYInput = "Mouse Y";
    public string joystickXInput = "RightStickHorizontal";
    public string joystickYInput = "RightStickVertical";

    // Called by Cinemachine to read input
    public float GetAxisValue(int axis)
    {
        switch (axis)
        {
            case 0: // Horizontal axis (X)
                float mouseX = Input.GetAxis(mouseXInput);
                float joystickX = Input.GetAxis(joystickXInput);
                return (mouseX + joystickX) * sensitivity;

            case 1: // Vertical axis (Y)
                float mouseY = Input.GetAxis(mouseYInput);
                float joystickY = Input.GetAxis(joystickYInput);
                return (mouseY + joystickY) * sensitivity;

            default:
                return 0f;
        }
    }
}

