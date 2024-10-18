using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    private void Start()
    {
        UpdatePostProcessing();
    }
    public void UpdatePostProcessing()
    {
        if(PlayerPrefs.GetInt("PostProcessing", 1) == 1)
        {
            GetComponent<PostProcessLayer>().enabled = true;
            GetComponent<PostProcessVolume>().enabled = true;
        }
        else
        {
            GetComponent<PostProcessLayer>().enabled = false;
            GetComponent<PostProcessVolume>().enabled = false;
        }
    }
}
