using UnityEngine;

public class CarSteering : MonoBehaviour
{
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private WheelCollider wheelFL;
    [SerializeField] private WheelCollider wheelFR;
    
    private CarInputHandler _input;

    private void Start() => _input = GetComponent<CarInputHandler>();

    private void FixedUpdate()
    {
        // Aplica o ângulo baseado no eixo X do input 
        float steer = _input.MoveInput.x * maxSteerAngle;
        wheelFL.steerAngle = steer;
        wheelFR.steerAngle = steer;
    }
}
