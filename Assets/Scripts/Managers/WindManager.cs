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
    public bool windChangeEnable = false;
    public bool randomizeStart = false;
    public float lerpingSpeed = 0.5f;

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
        windChangeTimer = 0;
        timeToFullTransition = 0;
        if (randomizeStart)
            RandomizeStart();
        else
        {
            wind = new Vector2(1, 0);
            windMagnitude = 4;
        }
    }

    public void SetWind(Vector2 direction, float magnitude)
    {
        wind = direction.normalized;
        windMagnitude = magnitude;
    }

    private void RandomizeStart()
    {
        wind = RandomizeWind();
        timeBeforeChange = Random.Range(minimumTimeBeforeChange, maximumTimeBeforeChange);
        windMagnitude = Random.Range(minimumWindMagnitude, maximumWindMagnitude);
        targetMagnitude = Random.Range(minimumWindMagnitude, maximumWindMagnitude);
        RotateWind();
    }

    private void Update()
    {
        if (windChangeEnable)
        {
            if (windChangeTimer < timeBeforeChange)
            {
                windChangeTimer += Time.deltaTime;
            }
            else
            {
                windChangeTimer = 0;
                timeBeforeChange = Random.Range(minimumTimeBeforeChange, maximumTimeBeforeChange);
                targetMagnitude = Random.Range(minimumWindMagnitude, maximumWindMagnitude);
                RotateWind();
            }
        }

        if (Vector2.Distance(targetDirection, wind) < 0.01f)
        {
            wind = Vector2.Lerp(wind, targetDirection, Time.deltaTime * lerpingSpeed);
        }

        if (Mathf.Abs(targetMagnitude - windMagnitude) < 0.1f)
        {
            windMagnitude = Mathf.Lerp(windMagnitude, targetMagnitude, Time.deltaTime * lerpingSpeed);
        }
    }

    public Vector2 RandomizeWind()
    {
        Vector2 newWind = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        if (newWind == Vector2.zero)
            return RandomizeWind();
        return newWind;
    }

    public void RotateWind()
    {
        targetDirection = Quaternion.Euler(0, 0, Random.Range(minimumWindChange, maximumWindChange)) * wind;
    }
}