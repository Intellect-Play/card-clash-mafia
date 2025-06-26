
using UnityEngine;

[CreateAssetMenu(fileName = "New Camera", menuName = "Camera")]
public class CameraSO : ScriptableObject
{
    public float duration = 0.8f;
    public float strength = 0.5f;
    public int vibrato = 10;
    public float randomness = 90f;
}
