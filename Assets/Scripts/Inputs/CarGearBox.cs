using UnityEngine;

/// <summary>
/// Sistema de transmissão:
/// - Define marchas
/// - Limite de velocidade por marcha
/// - Controle de troca com cooldown (evita pular marchas)
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
        -3.0f,
        0f,
        3.2f,
        2.1f,
        1.5f,
        1.0f,
        0.8f
    };

    // =========================
    // LIMITES DE VELOCIDADE
    // =========================

    [Header("Velocidade Máxima por Marcha")]
    [SerializeField] private float[] gearMaxSpeeds =
    {
        20f,
        0f,
        20f,
        40f,
        50f,
        60f,
        80f
    };

    [SerializeField] private Gear currentGear = Gear.First;

    // =========================
    // RPM
    // =========================

    [Header("RPM")]
    [SerializeField] private float minRPM = 800f;
    [SerializeField] private float maxRPM = 7000f;
    [SerializeField] private float rpmMultiplier = 50f;

    private float currentRPM = 0f;

    // =========================
    // CONTROLE DE TROCA
    // =========================

    [Header("Troca de Marcha")]
    
    /// <summary>
    /// Tempo mínimo entre trocas
    /// </summary>
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

        float speed = rb.linearVelocity.magnitude * 3.6f;

        Debug.Log($"Vel: {speed:0} km/h | Marcha: {currentGear} | RPM: {currentRPM:0}");
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

        if (ratio == 0)
        {
            currentRPM = Mathf.Lerp(currentRPM, minRPM, Time.fixedDeltaTime * 2f);
            return;
        }

        float targetRPM = speed * ratio * rpmMultiplier;
        targetRPM = Mathf.Clamp(targetRPM, minRPM, maxRPM);

        currentRPM = Mathf.Lerp(currentRPM, targetRPM, Time.fixedDeltaTime * 5f);
    }

    public float GetRPM() => currentRPM;

    public bool IsNeutral() => currentGear == Gear.Neutral;

    // =========================
    // CONTROLE DE TROCA (ANTI-SPAM)
    // =========================

    /// <summary>
    /// Verifica se pode trocar de marcha
    /// </summary>
    private bool CanShift()
    {
        return Time.time >= lastShiftTime + shiftCooldown;
    }

    public void ShiftUp()
    {
        // 🚨 Evita múltiplas trocas instantâneas
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