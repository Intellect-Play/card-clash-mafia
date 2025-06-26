using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public TextAsset levelJson;
    public AllLevelsData allLevelsData;
    private LevelData currentLevel;
    private int currentWaveIndex;
    public bool levelFinished { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        allLevelsData = JsonUtility.FromJson<AllLevelsData>(levelJson.text);
        //StartLevel(1); // Start with level 1
    }
    private void Start()
    {
        StartLevel(SaveManager.Instance.saveData.playerData.currentLevel);
    }
    public void StartLevel(int levelNumber)
    {
        currentLevel = allLevelsData.levels.FirstOrDefault(l => l.levelNumber == levelNumber);
        currentWaveIndex = 0;
        levelFinished = false;

        if (currentLevel == null)
        {
            Debug.LogError($"Level {levelNumber} not found in JSON!");
        }
    }

    public void SpawnNextWave()
    {
        if (currentLevel == null || levelFinished) return;
        if (currentWaveIndex < currentLevel.waves.Count)
        {
            List<EnemyData> wave = currentLevel.waves[currentWaveIndex].enemies;
            EnemySpawner.Instance.SpawnWave(wave);
            currentWaveIndex++;

            if (currentWaveIndex >= currentLevel.waves.Count)
            {
                levelFinished = true;
            }
        }
    }
}
