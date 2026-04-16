using UnityEngine;
using TMPro;

public class CarUI : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public TextMeshProUGUI speedText;

    public TextMeshProUGUI rpmText;
    public TextMeshProUGUI gearText;
    public CarGearbox gearbox;

    [Header("RPM Colors")]
    public float yellowThreshold = 0.85f;
    public float redThreshold = 0.95f;

    void Update()
    {
        // ===== VELOCIDADE =====
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f;
        speedText.text = Mathf.RoundToInt(speed) + " km/h";

        // ===== RPM =====
        float rpm = gearbox.CurrentRPM;
        rpmText.text = Mathf.RoundToInt(rpm) + " RPM";

        // 🔥 RPM baseado na marcha atual
        float maxRPMGear = gearbox.GetMaxRPMForCurrentGear();
        float rpmPercent = rpm / maxRPMGear;

        // ===== COR DO RPM =====
        if (rpmPercent >= redThreshold)
        {
            rpmText.color = Color.red;
        }
        else if (rpmPercent >= yellowThreshold)
        {
            rpmText.color = Color.yellow;
        }
        else
        {
            rpmText.color = Color.white;
        }

        // ===== MARCHA =====
        gearText.text = gearbox.GetGearString();
    }
}