﻿using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [Header("Colors")]
    public Color[] colors;
    public Color[] healthBarColors;
    public Color[] energyBarColors;

    [HideInInspector]
    public int spaceMineValue { get; set; }
    [HideInInspector]
    public int spaceMineForDurUpgrade = 7;
    [HideInInspector]
    public int spaceMineForSpeedUpgrade = 13;
    [HideInInspector]
    public int maxSpaceMine = 1;
    [HideInInspector]
    public bool showPcControlTutorial = true;

    private int gameOverCounter = 0;
    private float gamePausedTimeScale { get; set; }
    private static int point { get; set; }
    private static int enYuksekPuan { get; set; }


    public static GameControl gameManager { get; private set; } //basic singleton

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadGameData();
        LoadValues();
        LoadHighscore();
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        //SaveSystem.deleteDatas();
    }

    private void LoadValues()
    {
        point = 0;
        UIControl.UIManager.pointText.text = "Score: " + point;

        Time.timeScale = 1f;
        UIControl.UIManager.speedText.text = "Speed: " + Time.timeScale;

        UIControl.UIManager.spaceMineText.text = "x " + spaceMineValue;
    }

    #region Score and Highscore
    public void IncreaseScore(int value)
    {
        point += value;
        UIControl.UIManager.pointText.text = "Score: " + point;
    }

    private void LoadHighscore()
    {
        enYuksekPuan = PlayerPrefs.GetInt("enYuksekPuanKayit"); // en yüksek puan bilgimi çekiyorum.
    }

    private void AssignHighscore()
    {
        if (point > enYuksekPuan) // en yüksek puan için koşul
        {
            enYuksekPuan = point;
            PlayerPrefs.SetInt("enYuksekPuanKayit", enYuksekPuan); // en yüksek puanı kayıtlı tutuyoruz.
        }
        UIControl.UIManager.highScoreText.text = "High Score: " + PlayerPrefs.GetInt("enYuksekPuanKayit", enYuksekPuan); // en yuksek puanın gösterilmesi
    }
    #endregion

    public void GameSpeed(string state, float value)
    {
        if (state == "increase")
        {
            Time.timeScale += value;
        }
        else if(state == "reduce")
        {
            Time.timeScale -= value;
        }

        UIControl.UIManager.speedText.text = "Speed: " + System.Math.Round(Time.timeScale, 2);
    }

    public void SpaceMine(string state, int value)
    {
        if (state == "increase")
        {
            spaceMineValue += value;
        }
        else if (state == "reduce")
        {
            spaceMineValue -= value;
        }

        UIControl.UIManager.spaceMineText.text = "x " + spaceMineValue;
    }

    public void GameOver()
    {
        UIControl.UIManager.gameOverPanel.SetActive(true);
        AssignHighscore();
        UIControl.UIManager.pauseGameButton.SetActive(false);

        //check out mines for upgrade system
        UIControl.UIManager.UpgradeControl();

        //disable Pc Control tutorial
        UIControl.UIManager.pcControlTutorial.SetActive(false);
        showPcControlTutorial = false;

        //save game datas
        SaveGameData();

        //game over counter for ad
        gameOverCounter = PlayerPrefs.GetInt("oyunBittiSayac");
        gameOverCounter++;
        PlayerPrefs.SetInt("oyunBittiSayac", gameOverCounter);

        ShowAd();
    }

    #region Show Ad
    private void ShowAd()
    {
        if (PlayerPrefs.GetInt("oyunBittiSayac") == 3) //3 kere oyun bittiğinde reklam göster
        {
#if UNITY_ANDROID
            GameObject.FindGameObjectWithTag("reklamKontrolTag").GetComponent<AdControl>().ReklamiGoster();
#endif
            PlayerPrefs.SetInt("oyunBittiSayac", 0);
        }
    }
    #endregion

    #region Button Actions
    public void GoToMainMenu()
    {
        SceneControl.sceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        UIControl.UIManager.gameOverPanel.SetActive(false);
        SceneControl.sceneManager.LoadScene(1);
    }

    public void GamePauseAndUnpause() //oyunu durdur butonuna basıldığında
    {
        if (Time.timeScale > 0) //oyun devam ediyorsa
        {
            gamePausedTimeScale = Time.timeScale; //kalınan hız bilgisini al
            Time.timeScale = 0; // oyunu durdur
            UIControl.UIManager.pauseGameButton.GetComponent<Image>().sprite = UIControl.UIManager.gameContinueSprite;
            UIControl.UIManager.gamePausedPanel.SetActive(true);
            AudioListener.pause = true; //sesleri kapat
            UIControl.UIManager.changeColorImage.enabled = false; // oyun durduğunda ufonun rengi değiştirilemesin (mobil)
        }
        else if (Time.timeScale == 0) // oyun durmuşsa
        {
            Time.timeScale = gamePausedTimeScale; //oyuna kalınan hızdan devam et
            UIControl.UIManager.pauseGameButton.GetComponent<Image>().sprite = UIControl.UIManager.gamePauseSprite;
            UIControl.UIManager.gamePausedPanel.SetActive(false);
            AudioListener.pause = false;//sesi tekrar ac
            UIControl.UIManager.changeColorImage.enabled = true; // ufo rengi değiştirmeyi tekrar aktif et (mobil)
        }
    }
    #endregion

    #region Save and Load System
    public void SaveGameData()
    {
        SaveSystem.SaveGameData(this);
    }

    public void LoadGameData()
    {
        GameData gameData = SaveSystem.LoadGameData();

        if (gameData != null)
        {
            spaceMineValue = gameData.spaceMineValue;
            spaceMineForDurUpgrade = gameData.spaceMineForDurUpgrade;
            spaceMineForSpeedUpgrade = gameData.spaceMineForSpeedUpgrade;
            maxSpaceMine = gameData.maxSpaceMine;
            showPcControlTutorial = gameData.showPcControlTutorial;
        }
    }
    #endregion
}
