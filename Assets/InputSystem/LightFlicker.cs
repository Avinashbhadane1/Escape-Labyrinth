using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light lamp;
    public float minIntensity = 1.2f;
    public float maxIntensity = 1.6f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        lamp = GetComponent<Light>();
        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);
    }

    void Flicker()
    {
        lamp.intensity = Random.Range(minIntensity, maxIntensity);
    }
}
