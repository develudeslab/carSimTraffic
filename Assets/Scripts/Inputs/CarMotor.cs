using UnityEngine;

/// <summary>
/// Controla aceleração e aplica limite REAL de velocidade
/// </summary>
public class CarMotor : MonoBehaviour
{
    [Header("Motor")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float accelerationSpeed = 3f;
    [SerializeField] private float decelerationSpeed = 4f;
    [SerializeField] private float naturalDrag = 3f;

    [Header("Rodas")]
    [SerializeField] private WheelCollider wheelRL;
    [SerializeField] private WheelCollider wheelRR;

    private CarInputHandler _input;
    private Rigidbody rb;
    private CarGearbox gearbox;

    private float currentAcceleration = 0f;

    private void Start()
    {
        _input = GetComponent<CarInputHandler>();
        rb = GetComponent<Rigidbody>();
        gearbox = GetComponent<CarGearbox>();
    }

    private void FixedUpdate()
    {
        // 🚫 Neutro
        if (gearbox.CurrentGearEnum == CarGearbox.Gear.Neutral)
        {
            ForceNeutralState();
            return;
        }

        HandleAcceleration();
        ApplyMotorTorque();
        ApplyDrag();

        // Limita velocidade por marcha
        LimitSpeed();
    }

    /// <summary>
    /// Remove força do carro no neutro
    /// </summary>
    private void ForceNeutralState()
    {
        currentAcceleration = 0f;

        wheelRL.motorTorque = 0f;
        wheelRR.motorTorque = 0f;

        rb.linearDamping = naturalDrag;
    }

    private void HandleAcceleration()
    {
        float input = _input.MoveInput.y;

        if (input != 0)
        {
            currentAcceleration = Mathf.Lerp(currentAcceleration, input, accelerationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            currentAcceleration = Mathf.Lerp(currentAcceleration, 0f, decelerationSpeed * Time.fixedDeltaTime);
        }
    }

    private void ApplyMotorTorque()
    {
        float baseTorque = currentAcceleration * motorForce;
        float finalTorque = gearbox.GetTorque(baseTorque);

        wheelRL.motorTorque = finalTorque;
        wheelRR.motorTorque = finalTorque;
    }

    /// <summary>
    /// 🚨 LIMITADOR REAL DE VELOCIDADE POR MARCHA
    /// </summary>
    private void LimitSpeed()
    {
        float maxSpeed = gearbox.GetMaxSpeed();

        // Neutro ou inválido
        if (maxSpeed <= 0)
            return;

        float speed = rb.linearVelocity.magnitude * 3.6f;

        if (speed > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (maxSpeed / 3.6f);
        }
    }

    private void ApplyDrag()
    {
        float input = _input.MoveInput.y;
        rb.linearDamping = (input == 0) ? naturalDrag : 0f;
    }
}