using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    [SerializeField] private Text goldText; //gold amount UI text
    public int currGoldAmount; //current gold amount

    private void Start()
    {
        currGoldAmount = PlayerPrefs.GetInt("CurrentGold", 0);
        goldText.text = currGoldAmount.ToString(); //update UI
    }

    public void collectGold() //collecting gold
    {
        currGoldAmount++; //increase gold amount
        goldText.text = currGoldAmount.ToString(); //update UI
    }

    public void SaveGold() //saving gold amount in PlayerPrefs
    {
        int gold = PlayerPrefs.GetInt("Gold", 0);
        gold += currGoldAmount;
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.Save();
    }
}
