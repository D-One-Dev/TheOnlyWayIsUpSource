using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader; //SceneLoader script
    private void OnTriggerEnter2D(Collider2D collision) //triggering level end
    {
        if(collision.gameObject.CompareTag("Level End"))
        {
            PlayerPrefs.SetInt("CurrentGold", GetComponent<MoneyController>().currGoldAmount);
            PlayerPrefs.SetInt("CurrentHealth", GetComponent<PlayerHealth>().curHealth);
            PlayerPrefs.SetString("CurrentGun", GetComponent<PlayerShoot>().currGun.gunName);
            PlayerPrefs.SetInt("CurrentAmmo", GetComponent<PlayerShoot>().ammoLeft);

            sceneLoader.LoadNextScene();
        }
    }
}
