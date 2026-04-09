using UnityEngine;

/// <summary>
/// Responsável por todo o sistema de transmissão do carro:
/// - Controle de marchas
/// - Cálculo do torque final baseado na marcha
/// - Cálculo do RPM do motor
/// </summary>
public class CarGearbox : MonoBehaviour
{
    // =========================
    // CONFIGURAÇÃO DAS MARCHAS
    // =========================

    [Header("Marchas")]

    // Relações de marcha:
    // Índices:
    // 0 = Ré (valor negativo → movimento para trás)
    // 1 = Neutro (0 → sem transmissão de força)
    // 2+ = Marchas para frente
    [SerializeField] private float[] gears = { -3.0f, 0f, 3.2f, 2.1f, 1.5f, 1.0f, 0.8f };

    // Marcha atual do veículo
    [SerializeField] private int currentGear = 2;


    // =========================
    // CONFIGURAÇÃO DE RPM
    // =========================

    [Header("RPM")]

    // RPM mínimo do motor (marcha lenta)
    [SerializeField] private float minRPM = 800f;

    // RPM máximo (limite do motor)
    [SerializeField] private float maxRPM = 7000f;

    // Multiplicador usado para converter velocidade em RPM
    [SerializeField] private float rpmMultiplier = 50f;


    // =========================
    // VARIÁVEIS INTERNAS
    // =========================

    // Armazena o RPM atual do motor
    private float currentRPM = 0f;

    // Referência ao Rigidbody (usado para pegar velocidade do carro)
    private Rigidbody rb;


    // =========================
    // INICIALIZAÇÃO
    // =========================

    private void Start()
    {
        // Obtém o Rigidbody do carro
        rb = GetComponent<Rigidbody>();
    }


    // =========================
    // LOOP DE FÍSICA
    // =========================

    private void FixedUpdate()
    {
        // Atualiza o RPM continuamente

        Debug.Log("Marcha: " + currentGear);
        Debug.Log("RPM: " + currentRPM);
        CalculateRPM();
    }


    // =========================
    // CÁLCULO DE TORQUE FINAL
    // =========================

    /// <summary>
    /// Recebe o torque base do motor e aplica a relação da marcha atual.
    /// </summary>
    /// <param name="baseTorque">Torque gerado pelo motor (antes da transmissão)</param>
    /// <returns>Torque final aplicado às rodas</returns>
    public float GetTorque(float baseTorque)
    {
        // Obtém a relação da marcha atual
        float gearRatio = gears[currentGear];

        // Se estiver em neutro, não transmite força
        if (gearRatio == 0)
            return 0f;

        // Retorna o torque multiplicado pela marcha
        return baseTorque * gearRatio;
    }


    // =========================
    // CÁLCULO DE RPM
    // =========================

    /// <summary>
    /// Calcula o RPM do motor baseado na velocidade do carro e na marcha atual.
    /// </summary>
    private void CalculateRPM()
    {
        // Velocidade do carro convertida para km/h
        float speed = rb.linearVelocity.magnitude * 3.6f;

        // Relação da marcha atual
        float gearRatio = gears[currentGear];

        // Caso esteja em neutro
        if (gearRatio == 0)
        {
            // RPM tende ao mínimo (motor em marcha lenta)
            currentRPM = Mathf.Lerp(currentRPM, minRPM, Time.fixedDeltaTime * 2f);
            return;
        }

        // Calcula RPM alvo baseado na velocidade e marcha
        float targetRPM = speed * gearRatio * rpmMultiplier;

        // Limita RPM entre mínimo e máximo
        targetRPM = Mathf.Clamp(targetRPM, minRPM, maxRPM);

        // Suaviza a mudança de RPM (evita valores bruscos)
        currentRPM = Mathf.Lerp(currentRPM, targetRPM, Time.fixedDeltaTime * 5f);
    }


    // =========================
    // TROCA DE MARCHAS
    // =========================

    /// <summary>
    /// Troca para a próxima marcha (subir marcha).
    /// </summary>
    public void ShiftUp()
    {
        // Evita ultrapassar o limite superior
        if (currentGear < gears.Length - 1)
            currentGear++;
    }

    /// <summary>
    /// Troca para a marcha anterior (reduzir marcha).
    /// </summary>
    public void ShiftDown()
    {
        // Evita ultrapassar o limite inferior (ré)
        if (currentGear > 0)
            currentGear--;
    }


    // =========================
    // INPUT (PLAYER INPUT)
    // =========================

    /// <summary>
    /// Função chamada pelo PlayerInput ao pressionar o botão de subir marcha.
    /// </summary>
    public void OnShiftUp()
    {
        ShiftUp();
    }

    /// <summary>
    /// Função chamada pelo PlayerInput ao pressionar o botão de reduzir marcha.
    /// </summary>
    public void OnShiftDown()
    {
        ShiftDown();
    }


    // =========================
    // GETTERS
    // =========================

    /// <summary>
    /// Retorna a marcha atual.
    /// </summary>
    public int GetGear()
    {
        return currentGear;
    }

    /// <summary>
    /// Retorna o RPM atual do motor.
    /// </summary>
    public float GetRPM()
    {
        return currentRPM;
    }
}