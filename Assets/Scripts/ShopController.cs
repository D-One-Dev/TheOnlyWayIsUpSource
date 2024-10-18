using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Text goldText; //gold amount Text
    private int gold; //gold amount
    [SerializeField] private Suit[] suits;
    [SerializeField] private Gun[] guns;
    [SerializeField] private Image suitSprite;
    [SerializeField] private Button buyButton;
    [SerializeField] private Text suitHealth;
    [SerializeField] private Text suitSpeed;
    [SerializeField] private Image suitGun;
    private int usedSuit;
    private int currentSuit;
    public string boughtSuits;
    private void OnEnable()
    {
        gold = PlayerPrefs.GetInt("Gold", 0); //getting gold amount
        
        currentSuit = PlayerPrefs.GetInt("Suit", 0);
        usedSuit = currentSuit;

        boughtSuits = PlayerPrefs.GetString("BoughtSuits", "NONE");
        if(boughtSuits == "NONE")
        {
            boughtSuits = "1";
            for (int i = 1; i < suits.Length; i++) boughtSuits += "0";
        }
        else if(boughtSuits.Length < suits.Length)
        {
            string temp = "";
            for(int i = 0; i < boughtSuits.Length; i++)
            {
                temp += boughtSuits[i];
            }
            for(int j = 0; j < suits.Length - boughtSuits.Length; j++)
            {
                temp += "0";
            }
            boughtSuits = temp;
            PlayerPrefs.SetString("BoughtSuits", boughtSuits);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        goldText.text = gold.ToString(); //updating UI

        suitSprite.color = suits[currentSuit].color;

        suitHealth.text = suits[currentSuit].health.ToString();
        suitSpeed.text = suits[currentSuit].movementSpeed.ToString();
        if (suits[currentSuit].startGun != "Glock")
        {
            foreach(Gun gun in guns)
            {
                if (gun.gunName == suits[currentSuit].startGun)
                {
                    suitGun.sprite = gun.icon; 
                    break;
                }
            }
            suitGun.enabled = true;
        }
        else suitGun.enabled = false;

        if(currentSuit == usedSuit)
        {
            switch (LocalizationSettings.SelectedLocale.LocaleName)
            {
                case "English (en)":
                    buyButton.GetComponentInChildren<Text>().text = "EQUIPPED";
                    break;
                case "Russian (ru)":
                    buyButton.GetComponentInChildren<Text>().text = "ИСПОЛЬЗУЕТСЯ";
                    break;
                case "Spanish (es)":
                    buyButton.GetComponentInChildren<Text>().text = "USADO";
                    break;
                case "German (de)":
                    buyButton.GetComponentInChildren<Text>().text = "GEBRAUCHT";
                    break;
                case "French (fr)":
                    buyButton.GetComponentInChildren<Text>().text = "UTILISÉ";
                    break;
                case "Italian (it)":
                    buyButton.GetComponentInChildren<Text>().text = "USATO";
                    break;
                default:
                    buyButton.GetComponentInChildren<Text>().text = "EQUIPPED";
                    break;
            }
            buyButton.interactable = false;
        }
        else if (boughtSuits[currentSuit] == '0')
        {
            int cost = suits[currentSuit].cost;
            buyButton.GetComponentInChildren<Text>().text = cost.ToString();
            if(gold >= cost) buyButton.interactable = true;
            else buyButton.interactable = false;
        }
        else if (boughtSuits[currentSuit] == '1')
        {
            switch (LocalizationSettings.SelectedLocale.LocaleName)
            {
                case "English (en)":
                    buyButton.GetComponentInChildren<Text>().text = "EQUIP";
                    break;
                case "Russian (ru)":
                    buyButton.GetComponentInChildren<Text>().text = "ИСПОЛЬЗОВАТЬ";
                    break;
                case "Spanish (es)":
                    buyButton.GetComponentInChildren<Text>().text = "USAR";
                    break;
                case "German (de)":
                    buyButton.GetComponentInChildren<Text>().text = "VERWENDEN";
                    break;
                case "French (fr)":
                    buyButton.GetComponentInChildren<Text>().text = "UTILISER";
                    break;
                case "Italian (it)":
                    buyButton.GetComponentInChildren<Text>().text = "UTILIZZO";
                    break;
                default:
                    buyButton.GetComponentInChildren<Text>().text = "EQUIP";
                    break;

            }
            buyButton.interactable = true;
        }
    }

    public void NextSuit()
    {
        if(currentSuit < suits.Length - 1)
        {
            currentSuit++;
            UpdateUI();
        }
        else
        {
            currentSuit = 0;
            UpdateUI();
        }
    }

    public void PreviousSuit()
    {
        if (currentSuit == 0)
        {
            currentSuit = suits.Length - 1;
            UpdateUI();
        }
        else
        {
            currentSuit--;
            UpdateUI();
        }
    }

    public void Buy()
    {
        if (boughtSuits[currentSuit] == '0')
        {
            gold -= suits[currentSuit].cost;
            PlayerPrefs.SetInt("Gold", gold);

            char[] temp = boughtSuits.ToCharArray();
            temp[currentSuit] = '1';
            boughtSuits = "";
            foreach (char c in temp) boughtSuits += c;

            PlayerPrefs.SetString("BoughtSuits", boughtSuits);
        }
        usedSuit = currentSuit;
        PlayerPrefs.SetInt("Suit", currentSuit);
        UpdateUI();
    }

    public void ResetPurchases()
    {
        PlayerPrefs.SetInt("Suit", 0);

        string temp = "1";
        for(int i = 1; i < suits.Length; i++) temp += "0";
        PlayerPrefs.SetString("BoughtSuits", temp);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        PlayerPrefs.SetInt("Gold", gold);
        UpdateUI();
    }
}
