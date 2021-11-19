using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static bool leftGenoaGrabbed = false;
    public static bool rightGenoaGrabbed = false;
    public static bool mainSailGrabbed = false;
    public static bool tillerGrabbed = false;
    public static bool looking;
    public static bool interactionButton;

    public static Vector2 ropeDir;
    public static Vector2 tillerDir;
    public static Vector2 cameraDir;
    public static Vector2 playerDir;


    public void GrabLeft(InputAction.CallbackContext cx)
    {
        leftGenoaGrabbed = cx.ReadValueAsButton();
        rightGenoaGrabbed = cx.ReadValueAsButton();
    }

    public void LookingGlass(InputAction.CallbackContext cx)
    {
        looking = cx.ReadValueAsButton();
    }    
    public void Interaction(InputAction.CallbackContext cx)
    {
        interactionButton = cx.ReadValueAsButton();
    }

    public void GrabRight(InputAction.CallbackContext cx)
    {
        rightGenoaGrabbed = cx.ReadValueAsButton();
    }

    public void MainSail(InputAction.CallbackContext cx)
    {
        mainSailGrabbed = cx.ReadValueAsButton();
    }

    public void Reload(InputAction.CallbackContext cx)
    {
        SceneManager.LoadScene("BoatMechanics");
    }

    public void LeftStick(InputAction.CallbackContext cx)
    {
        if (leftGenoaGrabbed || mainSailGrabbed || rightGenoaGrabbed)
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
}