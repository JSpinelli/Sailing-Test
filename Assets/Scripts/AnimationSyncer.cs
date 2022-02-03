using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSyncer : MonoBehaviour
{
    public float startTime;
    public float endTime;
    
    private Animator myAnim;

    private bool isMoving = false;

    private bool playedForward = false;

    private bool playedBackward = false;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (GameManager.Instance.goingForward && startTime == GameManager.Instance.hour && !playedForward)
        // {
        //     playedForward = true;
        //     playedBackward = false;
        //     myAnim.SetTrigger("MoveForward");
        // }
        //
        // if (!GameManager.Instance.goingForward && endTime == GameManager.Instance.hour && !playedBackward){
        //     playedForward = false;
        //     playedBackward = true;
        //     myAnim.SetTrigger("MoveBackward");
        // }
    }
}
