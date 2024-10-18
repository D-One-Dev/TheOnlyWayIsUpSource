using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private readonly int LEVEL_COUNT = 3; //amount of levels in game
    public Animator transition; //transition animation
    [SerializeField] private float transitionTime = 1f; //transition time

    public void LoadScene(int sceneIndex) //loading scene by its index
    {
        Time.timeScale = 1;
        StartCoroutine(SceneLoad(sceneIndex));
    }

    public void LoadNextScene() //loading next scene (menu scene if current scene is last)
    {
        Time.timeScale = 1;
        int currScene = SceneManager.GetActiveScene().buildIndex;
        if (currScene == LEVEL_COUNT)
        {
            MoneyController moneyController = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyController>();
            moneyController.SaveGold();
        }
        //if (currScene < LEVEL_COUNT) StartCoroutine(SceneLoad(++currScene));
        StartCoroutine(SceneLoad(++currScene));
        //else StartCoroutine(SceneLoad(0));
    }

    private IEnumerator SceneLoad(int sceneIndex) // async scene loading
    {
        transition.SetTrigger("FadeOut");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(transitionTime);
        Time.timeScale = 1f;
        operation.allowSceneActivation = true;
    }
}
