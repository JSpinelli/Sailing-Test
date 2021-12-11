using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    
    public static bool leftTrigger = false;
    public static bool rightTrigger = false;
    public static bool tillerGrabbed = false;
    public static bool looking;
    public static bool interactionButton;
    public static bool rightBumper = false;
    public static bool leftBumper = false;
    public static bool paused = false;

    public static Vector2 ropeDir;
    public static Vector2 tillerDir;
    public static Vector2 cameraDir;
    public static Vector2 playerDir;

    public static PlayerInput inputs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Should not be another class");
            Destroy(this);
        }
    }
    
    private void Start()
    {
        inputs = GetComponent<PlayerInput>();
    }

    public void EnableUIInput()
    {
        inputs.SwitchCurrentActionMap("UI");
        //inputs.actions["UI"].Enable();
    }
    
    public void EnableBoatInput()
    {
        inputs.SwitchCurrentActionMap("Boat");
    }

    public void LeftTrigger(InputAction.CallbackContext cx)
    {
        if (!GameManager.Instance.autoFrontSailPositioning)
            leftTrigger = cx.ReadValueAsButton();
    }
    public void RightTrigger(InputAction.CallbackContext cx)
    {
        if (!GameManager.Instance.autoFrontSailPositioning)
            rightTrigger = cx.ReadValueAsButton();
    }
    
    public void LookingGlass(InputAction.CallbackContext cx)
    {
        looking = cx.ReadValueAsButton();
    }

    public void Interaction(InputAction.CallbackContext cx)
    {
        interactionButton = cx.ReadValueAsButton();
    }

    public void RightBumper(InputAction.CallbackContext cx)
    {
        if (!GameManager.Instance.autoMainSailPositioning)
            rightBumper = cx.performed;
    }

    public void LeftBumper(InputAction.CallbackContext cx)
    {
        if (!GameManager.Instance.autoMainSailPositioning)
            leftBumper = cx.performed;
    }

    public void LeftStick(InputAction.CallbackContext cx)
    {
        if (rightTrigger || leftTrigger)
        {
            ropeDir = (Vector2) cx.ReadValueAsObject();
            tillerDir = Vector2.zero;
            playerDir = Vector2.zero;
        }
        else if (tillerGrabbed)
        {
            ropeDir = Vector2.zero;
            playerDir = Vector2.zero;
            tillerDir = (Vector2) cx.ReadValueAsObject();
        }
        else
        {
            tillerDir = Vector2.zero;
            playerDir = (Vector2) cx.ReadValueAsObject();
            ropeDir = Vector2.zero;
        }
    }

    public void South(InputAction.CallbackContext cx)
    {
        tillerGrabbed = cx.ReadValueAsButton();
    }

    public void RightStick(InputAction.CallbackContext cx)
    {
        cameraDir = (Vector2) cx.ReadValueAsObject();
    }

    public void Pause(InputAction.CallbackContext cx)
    {
        if (cx.performed)
            GameManager.Instance.Pause();
    }
}