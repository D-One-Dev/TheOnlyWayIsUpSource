using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject; //player GameObject
    [SerializeField] private float ySmoothness; //Y axis movement smoothness
    [SerializeField] private GameObject cameraHolder; //camera holder GameObject
    void FixedUpdate() //moving camera to the player on Y axis
    {
        if (playerObject != null) //if player is alive
        { 
            float smoothY = Mathf.Lerp(transform.position.y, playerObject.transform.position.y, ySmoothness);
            cameraHolder.transform.position = new Vector3(0f, smoothY, -10f);
        }
    }

    private IEnumerator Shake(float duration, float strength) //shaking camera
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localPosition = new Vector3(Mathf.Sin(elapsed * 100) * strength, Mathf.Sin(elapsed * 100) * -strength, transform.localPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = new Vector3(0f, 0f, transform.localPosition.z);
    }

    public void ShakeCamera(float duration, float strength)
    {
        StartCoroutine(Shake(duration, strength));
    }
}
