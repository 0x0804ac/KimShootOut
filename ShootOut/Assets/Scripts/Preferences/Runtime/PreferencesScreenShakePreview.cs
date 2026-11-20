using UnityEngine;

//Use a dedicated settings-preview target, not a camera controlled by another shake system.
public sealed class PreferencesScreenShakePreview : MonoBehaviour
{
    [SerializeField]
    private Transform previewTarget;
    [SerializeField, Min(0.01f)]
    private float duration = 0.55f;
    [SerializeField, Min(0f)]
    private float maximumOffset = 0.20f;

    private Coroutine activeRoutine;
    private Vector3 originalLocalPosition;
    private bool hasOriginalPosition;

    private void OnDisable()
    {
        StopPreview();
    }

    public void Play(float normalizedIntensity)
    {
        if (previewTarget == null || normalizedIntensity <= 0f)
        {
            return;
        }

        StopPreview();
        originalLocalPosition = previewTarget.localPosition;
        hasOriginalPosition = true;
        activeRoutine = StartCoroutine(Animate(Mathf.Clamp01(normalizedIntensity)));
    }

    public void StopPreview()
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (previewTarget != null && hasOriginalPosition)
        {
            previewTarget.localPosition = originalLocalPosition;
        }

        hasOriginalPosition = false;
    }

    private System.Collections.IEnumerator Animate(float intensity)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float normalizedTime = Mathf.Clamp01(elapsed / duration);
            float amplitude = maximumOffset * intensity * (1f - normalizedTime);
            Vector2 direction = Random.insideUnitCircle * amplitude;

            previewTarget.localPosition = originalLocalPosition + new Vector3(direction.x, direction.y, 0f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        previewTarget.localPosition = originalLocalPosition;
        activeRoutine = null;
        hasOriginalPosition = false;
    }
}
