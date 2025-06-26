using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    public PlayerSaveData saveData;
    public CardDataList cardDataList;
    public string jsonPath;

    private void Awake()
    {
        if (Instance == null)
        {
            //ResetData();

            Instance = this;
            savePath = Application.persistentDataPath + "/saveDatas5.json";
            jsonPath = Path.Combine(Application.persistentDataPath, "Data/cards");
         //PlayerPrefs.DeleteAll(); // Test purposes, remove later
            LoadData();
            Load();
//saveData.playerData.coins = 50000; // Test purposes, remove later
            if (PlayerPrefs.GetInt("Tutorial2", 0) == 0)
            {
                ResetData();
            }
            //saveData.playerData.currentLevel = 2;
            //ResetData(); File.Delete(jsonPath);
            if (saveData.playerData.currentLevel>30) saveData.playerData.currentLevel = 2;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Data Load/Save Shop

    private void LoadData()
    {
        if (File.Exists(jsonPath))
        {
            Debug.Log("cards.json yüklendi: " + jsonPath);

            string json = File.ReadAllText(jsonPath);
            cardDataList = JsonUtility.FromJson<CardDataList>(json);
        }
        else
        {
            TextAsset jsonAsset = Resources.Load<TextAsset>("Data/cards");
            Debug.Log("cards.json " + jsonPath);

            if (jsonAsset != null)
            {
                Debug.Log("cards.json bulunamadı, varsayılan veriler yüklenecek: " + jsonPath);
                cardDataList = JsonUtility.FromJson<CardDataList>(jsonAsset.text);
                // File.WriteAllText(jsonPath, json);
            }
            else
            {
                Debug.Log("varsayılan veriler yüklenecek: " + jsonPath);

                // JSON yoksa 6 örnek kart yarat
                cardDataList = new CardDataList { cards = new List<CardData>() };
                for (int i = 0; i < 6; i++)
                {
                    cardDataList.cards.Add(new CardData
                    {
                        id = i,
                        name = "Card " + (i + 1),
                        power = 10,
                        isUnlocked = false,
                        buyCost = 100 + i * 50,
                        upgradeCost = 50 + i * 30
                    });
                }
                SaveData();
            }
        }
    }
    public void DeleteSaveFile()
    {
        jsonPath = Path.Combine(Application.persistentDataPath, "Data/cards.json");

        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
            Debug.Log("cards.json dosyası silindi: " + jsonPath);
        }
        else
        {
            Debug.LogWarning("Silinecek cards.json dosyası bulunamadı: " + jsonPath);
        }
    }


    public void SaveData()
    {
        Save();
        string directory = Path.GetDirectoryName(jsonPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonUtility.ToJson(cardDataList, true);
        File.WriteAllText(jsonPath, json);
    }


    #endregion
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<PlayerSaveData>(json);
            //File.Delete(savePath); // Yüklədikdən sonra faylı silirik
        }
        else
        {
            saveData = new PlayerSaveData(); // Default dəyərlərlə
            saveData.playerData.health = 4;
            saveData.playerData.coins = 0;
            saveData.playerData.currentLevel = 1;
            Save(); // İlk dəfə yaradıb saxlayırıq
        }
    }

    public void ResetData()
    {
        File.Delete(savePath);
        Load();
    }
}
