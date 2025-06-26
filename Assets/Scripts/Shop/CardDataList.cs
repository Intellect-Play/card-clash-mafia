using System.Collections.Generic;

[System.Serializable]
public class CardDataList
{
    public List<CardData> cards;
}
[System.Serializable]
public class CardData
{
    public int id;
    public string name;
    public int power;
    public int level;
    public bool isUnlocked;
    public int buyCost;
    public int upgradeCost;
    public bool update;
}
