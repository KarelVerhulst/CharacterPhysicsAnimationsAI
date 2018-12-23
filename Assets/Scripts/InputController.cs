using UnityEngine;

public class InputController {

    private static InputController _instance;

    public static InputController Instance()
    {
        if (_instance == null)
        {
            _instance = new InputController();
        }

        return _instance;
    }

    private InputController(){}
    /*
     *  All the input from the controllers are coded in this file
     */

    public Vector3 GetLeftJoystickInput()
    {
        float h = Input.GetAxis("Xbox_LeftJoystickHorizontal");
        float v = Input.GetAxis("Xbox_LeftJoystickVertical");

        return new Vector3(h, 0, v);
    }

    public Vector2 GetRightJoystickInput()
    {
        float h = Input.GetAxis("Xbox_RightJoystickHorizontal");
        float v = Input.GetAxis("Xbox_RightJoystickVertical");

        return new Vector2(h, v);
    }

    public bool IsLeftJoystickButtonPressed()
    {
        return Input.GetButtonDown("Xbox_LeftJoystickButton");
    }

    public bool IsButtonAPressed()
    {
        return Input.GetButtonDown("Xbox_A");
    }

    public bool IsButtonBPressed()
    {
        return Input.GetButtonDown("Xbox_B");
    }

    public bool IsButtonXPressed()
    {
        return Input.GetButtonDown("Xbox_X");
    }

    //this is a test if you don't have a controller
    public Vector3 KeyMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        

        return new Vector3(h,0, v);
    }
}
