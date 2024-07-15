using System.Collections;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.1f, 0.1f, 0.3f);
    public float dayIntensity = 1f;
    public float nightIntensity = 0.2f;
    public float transitionDuration = 2.0f; // Длительность перехода

    private bool isDay = true;
    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        isTransitioning = true;
        float elapsedTime = 0f;
        Color startColor = directionalLight.color;
        Color endColor = isDay ? nightColor : dayColor;
        float startIntensity = directionalLight.intensity;
        float endIntensity = isDay ? nightIntensity : dayIntensity;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            directionalLight.color = Color.Lerp(startColor, endColor, t);
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            yield return null;
        }

        directionalLight.color = endColor;
        directionalLight.intensity = endIntensity;
        isDay = !isDay;
        isTransitioning = false;
    }
}
