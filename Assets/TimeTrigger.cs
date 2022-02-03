using UnityEngine;

public class TimeTrigger : MonoBehaviour
{
    [RangeAttribute(0, 24)] public int hourStart;
    [RangeAttribute(0, 59)] public int minuteStart;
    
    public AnimationClip animClip;
    private int hourEnd;
    private int minuteEnd;

    private Animator myAnim;

    private bool goForward;
    private bool goBackward;
    private bool switchToBackward;
    private bool switchToForward;

    public float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        float targetEnd = animClip.length / TimeManager.Instance.secondsToMinute;
        hourEnd = hourStart + ((int) Mathf.Floor(targetEnd / 60));
        minuteEnd = minuteStart + ((int) targetEnd % 60);
        
        Debug.Log(hourEnd+" "+minuteEnd);
    }

    // Update is called once per frame
    void Update()
    {
        //SPEED VERSION
         if (TimeManager.Instance.goingForward &&
             hourStart == TimeManager.Instance.hour &&
             minuteStart == TimeManager.Instance.minutes)
         {
             myAnim.SetBool("MoveForward", true);
             
         }

         if (TimeManager.Instance.goingForward)
         {
             myAnim.SetBool("MoveBackward", false);
             myAnim.SetBool("GoingForward", true);
             myAnim.SetFloat("Speed", baseSpeed);
             myAnim.SetFloat("Speed2", -baseSpeed);
         }
         else
         {
             myAnim.SetBool("GoingForward", false);
             myAnim.SetBool("MoveForward", false);
             myAnim.SetFloat("Speed", -baseSpeed);
             myAnim.SetFloat("Speed2", baseSpeed);
         }

         
         if (!TimeManager.Instance.goingForward &&
             hourEnd == TimeManager.Instance.hour &&
             minuteEnd == TimeManager.Instance.minutes)
         {
             myAnim.SetBool("MoveBackward", true);
         }
    }
}