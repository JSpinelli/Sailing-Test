using TMPro;
using UnityEngine;

public class VisorManager : MonoBehaviour
{
    public static VisorManager Instance;
    
    public Camera cam;
    public float sizeOfRay;
    public float maxDistance;
    private bool active = false;

    public GameObject outline;
    public GameObject outlineFound;
    
    public GameObject dialog;
    public TextMeshProUGUI text;

    
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
        
        outline.SetActive(false);
        outlineFound.SetActive(false);
        dialog.SetActive(false);
    }
    
    private void Update()
    {
        if (active)
        {
            
            RaycastHit hit;
            Vector3 origin = cam.ScreenToWorldPoint(Vector3.zero);
            if (Physics.SphereCast(origin, sizeOfRay / 2, cam.transform.forward, out hit, maxDistance))
            {
                if (hit.transform.CompareTag("StoryBeacon"))
                {
                    outlineFound.SetActive(true);
                    dialog.SetActive(true);
                    text.text = hit.transform.GetComponent<StoryBeacon>().GetText();
                }
                else
                {
                    dialog.SetActive(false);
                    text.text = "";
                    outlineFound.SetActive(false);
                }
            }
            else
            {
                dialog.SetActive(false);
                text.text = "";
                outlineFound.SetActive(false);
            }
        }
    }

    public void ActivateVisor()
    {
        if (active)
        {
            active = false;
            // Powering Off effect
        }
        else
        {
            active = true;
            Debug.Log("Activated");
            // Powering On effect
        }
        outline.SetActive(active);
    }


}
