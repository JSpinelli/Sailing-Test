using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static bool leftGenoaGrabbed;
    public static bool rightGenoaGrabbed;
    public static bool mainSailGrabbed;

    public static Vector2 ropeDir;
    public static Vector2 tillerDir;
    public static Vector2 cameraDir;
    public static Vector2 playerDir;


    public void GrabLeft(InputAction.CallbackContext cx)
    {
        leftGenoaGrabbed = cx.ReadValueAsButton();
        rightGenoaGrabbed = cx.ReadValueAsButton();
    }

    public void GrabRight(InputAction.CallbackContext cx)
    {
        rightGenoaGrabbed = cx.ReadValueAsButton();
    }

    public void MainSail(InputAction.CallbackContext cx)
    {
        mainSailGrabbed = cx.ReadValueAsButton();
    }

    // public void ReleaseManual(InputAction.CallbackContext cx)
    // {
    //     if (leftGenoaGrabbed)
    //     {
    //         //Release / Catch Left Genoa
    //         if (leftGenoaLineAttached)
    //         {
    //             genoa.rope = 1;
    //             leftGenoaRope.text = " Left Genoa Detached";
    //         }
    //         else
    //         {
    //             genoa.rope = 0.1f;
    //             leftGenoaRope.text = "Left Genoa: " + genoa.rope;
    //         }
    //
    //         leftGenoaLineAttached = !leftGenoaLineAttached;
    //     }
    // }

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
        }
        else
        {
            ropeDir = Vector2.zero;
            tillerDir = (Vector2) cx.ReadValueAsObject();
        }
    }

    public void RightStick(InputAction.CallbackContext cx)
    {
        cameraDir = (Vector2) cx.ReadValueAsObject();
        playerDir = (Vector2) cx.ReadValueAsObject();
    }
}