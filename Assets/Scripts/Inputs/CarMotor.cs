using UnityEngine;

public class CarMotor : MonoBehaviour
{
    [Header("Motor")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float accelerationSpeed = 3f;
    [SerializeField] private float decelerationSpeed = 4f;
    [SerializeField] private float naturalDrag = 3f;

    [Header("Referências")]
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
        gearbox = GetComponent<CarGearbox>(); // pega o câmbio
    }

    private void FixedUpdate()
    {
        HandleAcceleration();
        ApplyMotorTorque();
        ApplyDrag();
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

        if (Mathf.Abs(currentAcceleration) < 0.01f)
            currentAcceleration = 0f;
    }

    private void ApplyMotorTorque()
    {
        float baseTorque = currentAcceleration * motorForce;

        // Aqui entra o câmbio
        float finalTorque = gearbox.GetTorque(baseTorque);

        wheelRL.motorTorque = finalTorque;
        wheelRR.motorTorque = finalTorque;
    }

    private void ApplyDrag()
    {
        float input = _input.MoveInput.y;

        rb.linearDamping = (input == 0) ? naturalDrag : 0f;
    }
}