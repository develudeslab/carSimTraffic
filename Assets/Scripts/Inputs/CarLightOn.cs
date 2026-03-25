using UnityEngine;

public class CarLightOn : MonoBehaviour
{
    public Light frontLight;

    public float maxIntensity = 900f;
    public float smoothSpeed = 10f;

    bool isLightOn;

    void Update()
    {
        isLightOn = Input.GetKey(KeyCode.F);

        // Suaviza a luz
        float target = isLightOn ? maxIntensity : 0f;
        frontLight.intensity = Mathf.Lerp(frontLight.intensity, target, Time.deltaTime * smoothSpeed);
    }
}