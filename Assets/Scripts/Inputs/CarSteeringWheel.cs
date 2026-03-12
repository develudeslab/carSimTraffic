using UnityEngine;

public class CarSteeringWheel : MonoBehaviour
{
    [SerializeField] private WheelCollider steerWheel;
    [SerializeField] private float steeringMultiplier = 15f;
    [SerializeField] private float smoothSpeed = 5f;

    float currentRotation;

    void Update()
    {
        float target = -steerWheel.steerAngle * steeringMultiplier;
        currentRotation = Mathf.Lerp(currentRotation, target, Time.deltaTime * smoothSpeed);

        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}