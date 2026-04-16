using UnityEngine;

/// <summary>
/// Sistema de transmissão:
/// - Define marchas
/// - Limite de velocidade por marcha
/// - Controle de troca com cooldown
/// - Cálculo de torque e RPM
/// </summary>
public class CarGearbox : MonoBehaviour
{
    // =========================
    // ENUM DE MARCHAS
    // =========================

    public enum Gear
    {
        Reverse = 0,
        Neutral = 1,
        First = 2,
        Second = 3,
        Third = 4,
        Fourth = 5,
        Fifth = 6
    }

    // =========================
    // RELAÇÃO DE MARCHAS
    // =========================

    [Header("Relação de Marchas")]
    [SerializeField] private float[] gears =
    {
        -3.0f, // Ré
        0f,    // Neutro
        3.2f,  // 1ª
        2.1f,  // 2ª
        1.5f,  // 3ª
        1.0f,  // 4ª
        0.8f   // 5ª
    };

    // =========================
    // LIMITES DE VELOCIDADE
    // =========================

    [Header("Velocidade Máxima por Marcha")]
    [SerializeField] private float[] gearMaxSpeeds =
    {
        20f, // Ré
        0f,  // Neutro
        20f, // 1ª
        40f, // 2ª
        50f, // 3ª
        60f, // 4ª
        80f  // 5ª
    };

    [SerializeField] private Gear currentGear = Gear.First;

    // =========================
    // ACESSO PARA OUTROS SCRIPTS
    // =========================

    public int CurrentGear => (int)currentGear;
    public Gear CurrentGearEnum => currentGear;

    // =========================
    // RPM
    // =========================

    [Header("RPM")]
    [SerializeField] private float minRPM = 800f;
    [SerializeField] private float maxRPM = 7000f;
    [SerializeField] private float rpmMultiplier = 50f;

    [SerializeField] private float currentRPM = 0f;

    public float CurrentRPM => currentRPM;
    public float MaxRPM => maxRPM;

    // =========================
    // TROCA DE MARCHA
    // =========================

    [Header("Troca de Marcha")]
    [SerializeField] private float shiftCooldown = 0.25f;

    private float lastShiftTime;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CalculateRPM();
    }

    // =========================
    // TORQUE
    // =========================

    public float GetTorque(float baseTorque)
    {
        float ratio = gears[(int)currentGear];

        if (ratio == 0)
            return 0f;

        return baseTorque * ratio;
    }

    // =========================
    // LIMITE DE VELOCIDADE
    // =========================

    public float GetMaxSpeed()
    {
        return gearMaxSpeeds[(int)currentGear];
    }

    // =========================
    // RPM
    // =========================

    private void CalculateRPM()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;
        float ratio = gears[(int)currentGear];

        // Neutro
        if (ratio == 0)
        {
            currentRPM = Mathf.Lerp(currentRPM, minRPM, Time.fixedDeltaTime * 2f);
            return;
        }

        float targetRPM = speed * Mathf.Abs(ratio) * rpmMultiplier;
        targetRPM = Mathf.Clamp(targetRPM, minRPM, maxRPM);

        currentRPM = Mathf.Lerp(currentRPM, targetRPM, Time.fixedDeltaTime * 5f);
    }

    /// <summary>
    /// 🔥 RPM máximo da marcha atual (usado na UI)
    /// </summary>
    public float GetMaxRPMForCurrentGear()
    {
        float maxSpeed = gearMaxSpeeds[(int)currentGear];
        float ratio = gears[(int)currentGear];

        if (ratio == 0)
            return minRPM;

        float maxGearRPM = maxSpeed * Mathf.Abs(ratio) * rpmMultiplier;
        return Mathf.Clamp(maxGearRPM, minRPM, maxRPM);
    }

    // =========================
    // UTILIDADES
    // =========================

    public string GetGearString()
    {
        if (currentGear == Gear.Neutral)
            return "N";

        if (currentGear == Gear.Reverse)
            return "R";

        return ((int)currentGear - 1).ToString();
    }

    // =========================
    // CONTROLE DE TROCA
    // =========================

    private bool CanShift()
    {
        return Time.time >= lastShiftTime + shiftCooldown;
    }

    public void ShiftUp()
    {
        if (!CanShift()) return;

        if ((int)currentGear < gears.Length - 1)
        {
            currentGear++;
            lastShiftTime = Time.time;
        }
    }

    public void ShiftDown()
    {
        if (!CanShift()) return;

        if ((int)currentGear > 0)
        {
            currentGear--;
            lastShiftTime = Time.time;
        }
    }

    public void OnShiftUp() => ShiftUp();
    public void OnShiftDown() => ShiftDown();
}