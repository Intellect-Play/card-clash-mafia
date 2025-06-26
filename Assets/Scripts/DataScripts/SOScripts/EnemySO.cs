
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    public int _HealthEnemy;
    public int _AttackDistance;
    public Sprite _EnemyImage;
}
