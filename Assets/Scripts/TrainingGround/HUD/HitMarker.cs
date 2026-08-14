using UnityEngine;
using System.Collections;

public class HitMarker : MonoBehaviour
{
    public float displayTime = 0.1f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        gameObject.SetActive(true);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        gameObject.SetActive(false);

        hideCoroutine = null;
    }
}