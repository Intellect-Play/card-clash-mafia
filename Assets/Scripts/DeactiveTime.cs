using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveTime : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(SetActiveFalseTime(2f)); // Set the time you want the object to be active
    }

    IEnumerator SetActiveFalseTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
