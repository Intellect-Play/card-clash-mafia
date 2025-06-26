using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardTypeScriptibleObject", menuName = "CardTypeMove/CardTypeScriptibleObject")]
public class CardTypeMove : ScriptableObject
{
    [Header("Info")]
    public Sprite CardIcon;
    public int MaxCardNumber;
    public int setAmount;
}
