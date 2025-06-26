using System;

[Serializable]
public class PlayerSaveData
{
    public PlayerData playerData = new PlayerData();
}

[Serializable]
public class PlayerData
{
    public int health;
    public int coins;
    public int currentLevel;
}
