using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Skin
{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}

public class SkinSelection : MonoBehaviour
{
    private LevelSelection _levelSelectionUI;
    private MainMenu _mainMenuUI;
    [SerializeField] private Skin[] _skinList;

    [Header("UI Details")]
    [SerializeField] private int _skinIndex;
    [SerializeField] private int _maxIndex;
    [SerializeField] private Animator _skinDisplay;

    [SerializeField] private TextMeshProUGUI _bankText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _buyText;

    private void Start()
    {
        LoadSkinsUnlocked();
        UpdateSkinDisplay();

        _mainMenuUI = GetComponentInParent<MainMenu>();
        _levelSelectionUI = _mainMenuUI.GetComponentInChildren<LevelSelection>(true);   
    }

    private void LoadSkinsUnlocked()
    {
        for (int i = 0; i < _skinList.Length; i++)
        {
            string skinName = _skinList[i].skinName;

            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

            if (skinUnlocked || i == 0)
                _skinList[i].unlocked = true;
        }
    }

    public void NextSkin()
    {
        _skinIndex++;
        if (_skinIndex > 3)
            _skinIndex = 0;

        UpdateSkinDisplay();
    }

    public void PreviousSkin()
    {
        _skinIndex--;
        if (_skinIndex < 0)
            _skinIndex = _maxIndex;

        UpdateSkinDisplay();
    }

    public void SelectSkin()
    {
        if (!_skinList[_skinIndex].unlocked)
            BuySkin(_skinIndex);
        else
        {
            SkinManager.Instance.SetSkinId(_skinIndex);
            _mainMenuUI.SwitchUI(_levelSelectionUI.gameObject);
        }

        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        _bankText.text = "Bank: " + FruitsInBank();
        for (int i = 0; i < _skinDisplay.layerCount; i++)
        {
            _skinDisplay.SetLayerWeight(i, 0);
        }

        _skinDisplay.SetLayerWeight(_skinIndex, 1);

        if (_skinList[_skinIndex].unlocked)
        {
            _priceText.transform.parent.gameObject.SetActive(false);
            _buyText.text = "Select";
        }
        else
        {
            _priceText.transform.parent.gameObject.SetActive(true);
            _priceText.text = "Price: " + _skinList[_skinIndex].skinPrice;
            _buyText.text = "Buy";
        }
        
    }
    private int FruitsInBank()
    {
        return PlayerPrefs.GetInt("TotalFruitsAmount");
    }

    private void BuySkin(int index)
    {
        if (!HaveEnoughFruits(_skinList[_skinIndex].skinPrice))
            return;

        string skinName = _skinList[_skinIndex].skinName;
        _skinList[_skinIndex].unlocked = true;

        PlayerPrefs.SetInt(skinName + "Unlocked", 1);
    }

    private bool HaveEnoughFruits(int price)
    {
        if (FruitsInBank() >= price)
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price);
            return true;
        }
        else
            return false; 
    }
}
