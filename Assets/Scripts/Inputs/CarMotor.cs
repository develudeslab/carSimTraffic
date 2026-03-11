using UnityEngine;

public class CarMotor : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float motorForce = 1500f;
    
    [Header("Referências")]
    [SerializeField] private WheelCollider wheelRL;
    [SerializeField] private WheelCollider wheelRR;
    
    private CarInputHandler _input;

    private void Start() => _input = GetComponent<CarInputHandler>();

    private void FixedUpdate()
    {
        // Aplica torque com base no eixo Y do input
        float torque = _input.MoveInput.y * motorForce;
        wheelRL.motorTorque = torque;
        wheelRR.motorTorque = torque;
    }
}
