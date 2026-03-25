using UnityEngine;

public class CarHeadLightSystem : MonoBehaviour
{
    public Light lowBeam;
    
    public Light highBeam;

    public float lowIntensity = 800f;
    public float highIntensity = 1500f;
    public float smoothSpeed = 10f;

    bool isLowOn = false;
    bool isHighOn = false;

    void Update()
    {
        // Tecla F = luz baixa
        if (Input.GetKeyDown(KeyCode.F))
        {
            isLowOn = !isLowOn;
            
            if (isLowOn)
                isHighOn = false; // desliga alta
        }

        // Tecla G = luz alta
        if (Input.GetKeyDown(KeyCode.G))
        {
            isHighOn = !isHighOn;

            if (isHighOn)
                isLowOn = false; // desliga baixa
        }

        // Aplicar intensidades com suavização
        float lowTarget = isLowOn ? lowIntensity : 0f;
        float highTarget = isHighOn ? highIntensity : 0f;

        lowBeam.intensity = Mathf.Lerp(lowBeam.intensity, lowTarget, Time.deltaTime * smoothSpeed);
        highBeam.intensity = Mathf.Lerp(highBeam.intensity, highTarget, Time.deltaTime * smoothSpeed);
    }
}