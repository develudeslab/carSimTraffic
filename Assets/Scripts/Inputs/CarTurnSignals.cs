using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CarTurnSignalSystem : MonoBehaviour
{
    [Header("Blink Lights")]
    public List<Light> leftSignals;   // Todas as luzes da esquerda
    public List<Light> rightSignals;  // Todas as luzes da direita

    [Header("Blink Settings")]
    public float blinkIntensity = 4f;
    public float blinkInterval = 0.5f;

    private bool leftOn = false;
    private bool rightOn = false;
    private bool hazardOn = false;

    private float blinkTimer = 0f;
    private bool blinkState = false;

    public void OnLeftSignal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            leftOn = !leftOn;

            if (leftOn)
            {
                rightOn = false;
                hazardOn = false;
            }
        }
    }

    public void OnRightSignal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rightOn = !rightOn;

            if (rightOn)
            {
                leftOn = false;
                hazardOn = false;
            }
        }
    }

    public void OnHazard(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            hazardOn = !hazardOn;

            if (hazardOn)
            {
                leftOn = false;
                rightOn = false;
            }
        }
    }

    void Update()
    {
        UpdateBlinkers();
    }

    void UpdateBlinkers()
    {
        if (!leftOn && !rightOn && !hazardOn)
        {
            SetBlinkLights(0f, 0f);
            return;
        }

        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            blinkState = !blinkState;
        }

        float value = blinkState ? blinkIntensity : 0f;

        if (hazardOn)
        {
            SetBlinkLights(value, value);
        }
        else
        {
            float leftValue = leftOn ? value : 0f;
            float rightValue = rightOn ? value : 0f;
            SetBlinkLights(leftValue, rightValue);
        }
    }

    void SetBlinkLights(float left, float right)
    {
        foreach (Light l in leftSignals)
        {
            if (l != null)
                l.intensity = left;
        }

        foreach (Light l in rightSignals)
        {
            if (l != null)
                l.intensity = right;
        }
    }
}