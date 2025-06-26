using System;
using System.Collections.Generic;

[Serializable]
public class AllLevelsData
{
    public List<LevelData> levels;
}

[Serializable]
public class LevelData
{
    public int levelNumber;
    public List<WaveData> waves;
}

[Serializable]
public class WaveData
{
    public float delay;
    public List<EnemyData> enemies;
}

[Serializable]
public class EnemyData
{
    public string type;
    public int column;
}
