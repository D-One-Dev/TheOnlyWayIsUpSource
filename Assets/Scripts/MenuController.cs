using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider FPSSlider; //FPS slider
    [SerializeField] private Text FPSValue; //FPS value text
    [SerializeField] private Toggle soundsToggle; //sounds toggle
    [SerializeField] private Toggle musicToggle; //music toggle
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private AudioMixerGroup Mixer;
    [SerializeField] private AudioSource AS;
    [SerializeField] private SceneLoader sceneLoader; //SceneLoader script
    [SerializeField] private PostProcessingController postProcessing;
    public int FPS;
    private int LANGUAGES_AMOUNT = 6;
    public int currentLanguage;
    private void OnEnable()
    {
        //getting saved preferences
        currentLanguage = PlayerPrefs.GetInt("Locale", -1);
        if(currentLanguage == -1)
        {
            switch (LocalizationSettings.SelectedLocale.LocaleName)
            {
                case "English (en)":
                    currentLanguage = 0;
                    break;
                case "Russian (ru)":
                    currentLanguage = 1;
                    break;
                case "Spanish (es)":
                    currentLanguage = 2;
                    break;
                case "German (de)":
                    currentLanguage = 3;
                    break;
                case "French (fr)":
                    currentLanguage = 4;
                    break;
                case "Italian (it)":
                    currentLanguage = 5;
                    break;
                default:
                    currentLanguage = 1;
                    break;

            }
        }
        StartCoroutine(LocalizationChangeDelay());

        FPSSlider.value = PlayerPrefs.GetInt("FPS", 0);
        FPSSliderChange();
        if (PlayerPrefs.GetInt("Music", 1) == 1) musicToggle.isOn = true;
        else musicToggle.isOn = false;
        if (PlayerPrefs.GetInt("Sounds", 1) == 1) soundsToggle.isOn = true;
        else soundsToggle.isOn = false;
        if (PlayerPrefs.GetInt("PostProcessing", 1) == 1) postProcessingToggle.isOn = true;
        else postProcessingToggle.isOn = false;
    }
    public void StartGame() //starting new game
    {
        PlayerPrefs.SetInt("CurrentGold", 0);
        PlayerPrefs.SetInt("CurrentHealth", -1);
        PlayerPrefs.SetString("CurrentGun", "Default");
        PlayerPrefs.SetInt("CurrentAmmo", 0);

        sceneLoader.LoadScene(1);
    }

    public void ExitToMenu() //going to main menu
    {
        sceneLoader.LoadScene(0);
    }

    public void FPSSliderChange() //on FPS slider value change
    {
        switch (FPSSlider.value)
        {
            case 0: //framerate = 60
                FPS = 60;
                //FPSValue.text = "FPS: 60";
                break;
            case 1: //framerate = 90
                FPS = 90;
                //FPSValue.text = "FPS: 90";
                break;
            case 2: //framerate = 120
                FPS = 120;
                //FPSValue.text = "FPS: 120";
                break;
            case 3: //framerate = 144
                FPS = 144;
                //FPSValue.text = "FPS: 120";
                break;
            default: //error
                FPS = 60;
                //FPSValue.text = "Unknown error";
                break;
        }
        Application.targetFrameRate = FPS;
        FPSValue.text = "FPS: " + FPS.ToString();
        PlayerPrefs.SetInt("FPS", Mathf.RoundToInt(FPSSlider.value)); //saving value
        PlayerPrefs.Save();
    }

    public void SoundsToggleChanged() //on sounds toggle change
    {
        if (soundsToggle.isOn) //enabling sounds
        {
            Mixer.audioMixer.SetFloat("EffectsVolume", 0);
            Mixer.audioMixer.SetFloat("UIVolume", 0);
            PlayerPrefs.SetInt("Sounds", 1);
            PlayerPrefs.Save();
        }
        else //disabling sounds
        {
            Mixer.audioMixer.SetFloat("EffectsVolume", -80);
            Mixer.audioMixer.SetFloat("UIVolume", -80);
            PlayerPrefs.SetInt("Sounds", 0);
            PlayerPrefs.Save();
        }
    }

    public void MusicToggleChanged() //on music toggle change
    {
        if (musicToggle.isOn) //enabling music
        {
            Mixer.audioMixer.SetFloat("MusicVolume", 0);
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.Save();
        }
        else //disabling music
        {
            Mixer.audioMixer.SetFloat("MusicVolume", -80);
            PlayerPrefs.SetInt("Music", 0);
            PlayerPrefs.Save();
        }
    }

    public void PostProcessingToggleChanged() //on post processing toggle change
    {
        if (postProcessingToggle.isOn) //enabling post processing
        {
            PlayerPrefs.SetInt("PostProcessing", 1);
            PlayerPrefs.Save();

        }
        else //disabling post processing
        {
            PlayerPrefs.SetInt("PostProcessing", 0);
            PlayerPrefs.Save();
        }

        postProcessing.UpdatePostProcessing();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void PlayButtonSound() //playing random button sound
    {
         AS.pitch = Random.Range(.8f, 1.2f);
         AS.Play();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeLanguage()
    {
        currentLanguage++;
        if (currentLanguage >= LANGUAGES_AMOUNT) currentLanguage = 0;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLanguage];
        PlayerPrefs.SetInt("Locale", currentLanguage);
    }

    private IEnumerator LocalizationChangeDelay()
    {
        yield return new WaitForSeconds(.1f);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLanguage];
    }
}
