using UnityEngine;
using System.Collections;

public class PerlinNoise : MonoBehaviour 
{
    public float duration = 0.5f;
    public float speed = 1.0f;
    public float magnitude = 0.1f;

    public static PerlinNoise instance;

    void Awake()
    {
        instance = this;
    }

    public void Shake(Transform target)
    {
        StartCoroutine(PlayShake(target));
    }


    IEnumerator PlayShake(Transform target)
    {

        float elapsed = 0.0f;

        Vector3 originalCamPos = target.position;
        float randomStart = Random.Range(-1000.0f, 1000.0f);

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;

            // We want to reduce the shake from full power to 0 starting half way through
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            // Calculate the noise parameter starting randomly and going as fast as speed allows
            float alpha = randomStart + speed * percentComplete;

            // map noise to [-1, 1]
            float x = Noise.GetNoise(alpha, 0.0f, 0.0f) * 2.0f - 1.0f;
            float y = Noise.GetNoise(0.0f, alpha, 0.0f) * 2.0f - 1.0f;

            x *= magnitude * damper;
            y *= magnitude * damper;

            target.position = new Vector3(x, y, originalCamPos.z);

            yield return null;
        }

        target.position = originalCamPos;
    }
}
