using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    // Subscriptions

    private List<Action> _leftTrigger = new List<Action>();
    private List<Action> _rightTrigger = new List<Action>();

    private List<Action> _leftBumper = new List<Action>();
    private List<Action> _rightBumper = new List<Action>();

    private List<Action> _westButton = new List<Action>();
    private List<Action> _northButton = new List<Action>();
    private List<Action> _southButton = new List<Action>();
    private List<Action> _eastButton = new List<Action>();

    private List<Action> leftAnalog = new List<Action>();
    private List<Action> rightAnalog = new List<Action>();

    public static Vector2 leftAnalogPos;
    public static Vector2 rightAnalogPos;

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
    // Subscribers

    public void Subcribe(string key, Action action)
    {
        switch (key)
        {
            case "leftTrigger":
            {
                _leftTrigger.Add(action);
                break;
            }
            case "rightTrigger":
            {
                _rightTrigger.Add(action);
                break;
            }
            case "leftBumper":
            {
                _leftBumper.Add(action);
                break;
            }
            case "rightBumper":
            {
                _rightBumper.Add(action);
                break;
            }
            case "north":
            {
                _northButton.Add(action);
                break;
            }
            case "east":
            {
                _eastButton.Add(action);
                break;
            }
            case "west":
            {
                _westButton.Add(action);
                break;
            }
            case "south":
            {
                _southButton.Add(action);
                break;
            }
            case "leftAnalog":
            {
                leftAnalog.Add(action);
                break;
            }
            case "rightAnalog":
            {
                rightAnalog.Add(action);
                break;
            }
        }
    }

    // Triggers
    public void LeftDPad(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
        }
    }

    public void RightDPad(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
        }
    }

    public void UpDPad(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
        }
    }

    public void DownDPad(InputAction.CallbackContext cx)
    {
    }

    public void North(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_northButton.Count > 0)
            {
                _northButton[0].Invoke();
            }
        }
    }

    public void West(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_westButton.Count > 0)
            {
                _westButton[0].Invoke();
            }
        }
    }

    public void East(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_eastButton.Count > 0)
            {
                _eastButton[0].Invoke();
            }
        }
    }

    public void South(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_southButton.Count > 0)
            {
                _southButton[0].Invoke();
            }
        }
    }

    public void LeftTrigger(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_leftTrigger.Count > 0)
            {
                _leftTrigger[0].Invoke();
            }
        }
    }

    public void RightTrigger(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_rightTrigger.Count > 0)
            {
                _rightTrigger[0].Invoke();
            }
        }
    }

    public void RightBumper(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_rightBumper.Count > 0)
            {
                _rightBumper[0].Invoke();
            }
        }
    }

    public void LeftBumper(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_leftBumper.Count > 0)
            {
                _leftBumper[0].Invoke();
            }
        }
    }
}