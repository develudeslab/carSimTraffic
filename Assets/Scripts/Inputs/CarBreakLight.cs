using UnityEngine;

public class CarBrakeLight : MonoBehaviour
{
    public Light brakeLight;

    public float maxIntensity = 3f;
    public float smoothSpeed = 10f;

    private CarInputHandler _input;

    private void Start() => _input = GetComponent<CarInputHandler>();

    void Update()
    {

        // Suaviza a luz
        float target = _input.IsBraking ? maxIntensity : 0f;
        brakeLight.intensity = Mathf.Lerp(brakeLight.intensity, target, Time.deltaTime * smoothSpeed);
    }
}