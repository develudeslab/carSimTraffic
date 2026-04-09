using UnityEngine;

public class CarMotor : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float accelerationSpeed = 3f;
    [SerializeField] private float decelerationSpeed = 4f;
    [SerializeField] private float naturalDrag = 3f;

    [Header("Referências")]
    [SerializeField] private WheelCollider wheelRL;
    [SerializeField] private WheelCollider wheelRR;

    private CarInputHandler _input;
    private Rigidbody rb;

    private float currentAcceleration = 0f;

    private void Start()
    {
        _input = GetComponent<CarInputHandler>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float input = _input.MoveInput.y;

        // Aceleração / desaceleração
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

        // Evita valores muito pequenos (carro "andando sozinho")
        if (Mathf.Abs(currentAcceleration) < 0.01f)
        {
            currentAcceleration = 0f;
        }

        // Aplica torque
        float torque = currentAcceleration * motorForce;
        wheelRL.motorTorque = torque;
        wheelRR.motorTorque = torque;

        // Drag dinâmico 
        if (input == 0)
        {
            rb.linearDamping = naturalDrag; // resistência ao soltar
        }
        else
        {
            rb.linearDamping = 0f; // livre pra acelerar
        }
    }
}