using UnityEngine;

public class StoryBeacon : MonoBehaviour
{
    public int islandNumber;
    public int beaconNumber;

    public string content;

    public string GetText()
    {
        return content;
    }
}
