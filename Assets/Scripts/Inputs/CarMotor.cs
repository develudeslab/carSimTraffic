using UnityEngine;

public class CarMotor : MonoBehaviour
{
    // =========================
    // CONFIGURAÇÕES DO MOTOR
    // =========================
    [Header("Configurações do Motor")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float accelerationSpeed = 3f;
    [SerializeField] private float decelerationSpeed = 4f;
    [SerializeField] private float naturalDrag = 3f;

    // =========================
    // MARCHAS
    // =========================
    [Header("Marchas")]
    [SerializeField] private float[] gears = { -3.0f, 0f, 3.2f, 2.1f, 1.5f, 1.0f, 0.8f };
    [SerializeField] private int currentGear = 2;

    // =========================
    // RPM
    // =========================
    [Header("RPM")]
    [SerializeField] private float minRPM = 800f;
    [SerializeField] private float maxRPM = 7000f;
    [SerializeField] private float rpmMultiplier = 50f;

    // =========================
    // REFERÊNCIAS
    // =========================
    [Header("Referências")]
    [SerializeField] private WheelCollider wheelRL;
    [SerializeField] private WheelCollider wheelRR;

    private CarInputHandler _input;
    private Rigidbody rb;

    private float currentAcceleration = 0f;
    private float currentRPM = 0f;

    private void Start()
    {
        _input = GetComponent<CarInputHandler>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Debug.Log("Marcha: " + currentGear + " | RPM: " + currentRPM);
        HandleAcceleration();
        ApplyMotorTorque();
        ApplyDrag();
        CalculateRPM();
    }

    // =========================
    // ACELERAÇÃO
    // =========================
    private void HandleAcceleration()
    {
        float input = _input.MoveInput.y;

        if (input != 0)
        {
            currentAcceleration = Mathf.Lerp(
                currentAcceleration,
                input,
                accelerationSpeed * Time.fixedDeltaTime
            );
        }
        else
        {
            currentAcceleration = Mathf.Lerp(
                currentAcceleration,
                0f,
                decelerationSpeed * Time.fixedDeltaTime
            );
        }

        if (Mathf.Abs(currentAcceleration) < 0.01f)
        {
            currentAcceleration = 0f;
        }
    }

    // =========================
    // TORQUE
    // =========================
    private void ApplyMotorTorque()
    {
        float gearRatio = gears[currentGear];

        // Neutro
        if (gearRatio == 0)
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
            return;
        }

        float torque = currentAcceleration * motorForce * gearRatio;

        float speed = rb.linearVelocity.magnitude * 3.6f;
        float maxSpeed = 30f * currentGear;

        if (speed > maxSpeed)
        {
            torque *= 0.2f;
        }

        wheelRL.motorTorque = torque;
        wheelRR.motorTorque = torque;
    }

    // =========================
    // DRAG
    // =========================
    private void ApplyDrag()
    {
        float input = _input.MoveInput.y;

        if (input == 0)
        {
            rb.linearDamping = naturalDrag;
        }
        else
        {
            rb.linearDamping = 0f;
        }
    }

    // =========================
    // RPM
    // =========================
    private void CalculateRPM()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;
        float gearRatio = gears[currentGear];

        if (gearRatio == 0)
        {
            currentRPM = Mathf.Lerp(currentRPM, minRPM, Time.fixedDeltaTime * 2f);
            return;
        }

        float targetRPM = speed * gearRatio * rpmMultiplier;
        targetRPM = Mathf.Clamp(targetRPM, minRPM, maxRPM);

        currentRPM = Mathf.Lerp(currentRPM, targetRPM, Time.fixedDeltaTime * 5f);
    }

    // =========================
    // INPUT VIA PLAYER INPUT (IMPORTANTE)
    // =========================

    // Chamado pelo PlayerInput → ShiftUp
    public void OnShiftUp()
{
    Debug.Log("Shift Up pressionado");
    if (currentGear < gears.Length - 1)
        currentGear++;
}

public void OnShiftDown()
{
    Debug.Log("Shift Down pressionado");
    if (currentGear > 0)
        currentGear--;
}

    // =========================
    // GETTERS
    // =========================
    public int GetCurrentGear()
    {
        return currentGear;
    }

    public float GetCurrentRPM()
    {
        return currentRPM;
    }

    
}