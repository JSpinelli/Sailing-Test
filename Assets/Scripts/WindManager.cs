using UnityAtoms.BaseAtoms;
using UnityEngine;

[ExecuteInEditMode]
public class WindManager : MonoBehaviour
{
    public static WindManager instance;
    public Vector2 wind;
    public float windMagnitude;
    
    public float minimumTimeBeforeChange;
    public float maximumTimeBeforeChange;
    
    public float maximumWindChange = 180;
    public float minimumWindChange = 10;
    
    public float maximumWindMagnitude = 10;
    public float minimumWindMagnitude = 1;

    public float transitionTime = 2;
    
    
    public float noGo = -0.45f;
    
    
    private float timeBeforeChange;
    private float windChangeTimer;
    private float targetMagnitude;
    private Vector2 targetDirection;
    private float timeToFullTransition = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Should not be another class");
            Destroy(this);
        }
    }

    private void Start()
    {
        wind = RandomizeWind();
        timeBeforeChange = Random.Range(minimumTimeBeforeChange, maximumTimeBeforeChange);
        windMagnitude = Random.Range(minimumWindMagnitude, maximumWindMagnitude);
        windChangeTimer = 0;
        timeToFullTransition = 0;
        targetMagnitude = Random.Range(minimumWindMagnitude, maximumWindMagnitude);
        RotateWind();
    }

    private void Update()
    {
        if (windChangeTimer < timeBeforeChange)
        {
            windChangeTimer += Time.deltaTime;
        }
        else
        {
            if (timeToFullTransition < transitionTime)
            {
                windMagnitude = Mathf.Lerp(windMagnitude, targetMagnitude, Time.deltaTime);
                wind = Vector2.Lerp(wind, targetDirection, Time.deltaTime);
                timeToFullTransition += Time.deltaTime;
            }
            else
            {
                windChangeTimer = 0;
                timeToFullTransition = 0;
                timeBeforeChange = Random.Range(minimumTimeBeforeChange, maximumTimeBeforeChange);
                targetMagnitude = Random.Range(minimumWindMagnitude, maximumWindMagnitude);
                RotateWind();
            }
        }
    }

    public Vector2 GetWind()
    {
        return wind.normalized;
    }

    public Vector2 RandomizeWind()
    {
        return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        
    }

    public void RotateWind()
    {
        targetDirection = Quaternion.Euler(0, 0, Random.Range(minimumWindChange, maximumWindChange)) * wind;
    }
}
