using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // degrees per second

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
