using UnityEngine;

public class EndScreenController : MonoBehaviour
{
    public void GoToMenu()
    {
        GetComponent<SceneLoader>().LoadScene(0);
    }

    public void RestartGame()
    {
        GetComponent<SceneLoader>().LoadScene(1);
    }
}
