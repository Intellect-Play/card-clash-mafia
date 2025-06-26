
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public CardType _CardType;
    public Sprite _CardImage;
    public GameObject _CardCost;
    public int _CardPower;
    public int _CardRange;
    public Vector3Int _CardPoweraArea;
    //public List<CardEffect> _CardEffects = new List<CardEffect>();
}

