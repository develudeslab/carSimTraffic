using UnityEngine;

public class CarBraking : MonoBehaviour
{
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private WheelCollider[] allWheels; // Lista para facilitar aplicação em massa
    
    private CarInputHandler _input;

    private void Start() => _input = GetComponent<CarInputHandler>();

    private void FixedUpdate()
    {
        float currentBrake = _input.IsBraking ? brakeForce : 0f;
        
        foreach (var wheel in allWheels)
        {
            wheel.brakeTorque = currentBrake;
        }
    }
}
