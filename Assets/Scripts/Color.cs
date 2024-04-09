using System.Collections;
using UnityEngine;

public class Color : MonoBehaviour
{
    Animator animator;
    public int color;
    public int numRow, numCol;
    public void AppearBlock(float appearDuration)
    {
        StartCoroutine(AppearEffect(appearDuration));
    }

    private IEnumerator AppearEffect(float appearDuration)
    {
        Vector3 targetScale  = transform.localScale;
        Vector3 initialScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < appearDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / appearDuration);
            yield return null;
        }

        transform.localScale = targetScale;

    }

    public void MoveDown(float distance, float lerpDuration)
    {
        StartCoroutine(MoveDownEffect(distance, lerpDuration));
    }

    IEnumerator MoveDownEffect(float distance, float lerpDuration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = transform.position + Vector3.down * distance * 0.585f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpDuration);
            yield return null;
        }

        transform.position = targetPosition;
    }
    public void DestroyBlock(float lerpDuration)
    {
        StartCoroutine(DestroyEffect(lerpDuration));
    }

    public IEnumerator DestroyEffect(float lerpDuration)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / lerpDuration);
            yield return null;
        }

        transform.localScale = targetScale;

        Destroy(gameObject);
    }
    
}
