using UnityEngine;

public class CarBrakeLight : MonoBehaviour
{
    public Light brakeLight;

    public float maxIntensity = 3f;
    public float smoothSpeed = 10f;

    bool isBraking;

    void Update()
    {
        isBraking = Input.GetKey(KeyCode.Space);

        // Suaviza a luz
        float target = isBraking ? maxIntensity : 0f;
        brakeLight.intensity = Mathf.Lerp(brakeLight.intensity, target, Time.deltaTime * smoothSpeed);
    }
}