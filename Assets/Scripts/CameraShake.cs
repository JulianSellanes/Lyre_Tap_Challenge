using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public float duration = 0.3f;
    public float magnitude = 0.05f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void StartShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}