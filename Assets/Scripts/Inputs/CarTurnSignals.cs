using UnityEngine;
using UnityEngine.InputSystem;

// Sistema de setas do carro:
// - Seta esquerda
// - Seta direita
// - Pisca-alerta
// Usa PlayerInput (Invoke Unity Events)

public class CarTurnSignalSystem : MonoBehaviour
{
    [Header("Blink Lights")]
    public Light leftSignal;   // Luz da seta esquerda
    public Light rightSignal;  // Luz da seta direita

    [Header("Blink Settings")]
    public float blinkIntensity = 4f;   // Intensidade da luz
    public float blinkInterval = 0.5f;  // Tempo entre piscar

    // Estados das setas
    private bool leftOn = false;
    private bool rightOn = false;
    private bool hazardOn = false;

    // Controle do pisca
    private float blinkTimer = 0f;
    private bool blinkState = false;

    // ===== INPUT EVENTS =====

    // Seta esquerda (tecla Q)
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

    // Seta direita (tecla E)
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

    // Pisca-alerta (tecla H)
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

    // ===== UPDATE =====
    void Update()
    {
        UpdateBlinkers();
    }

    // Controla o piscar das luzes
    void UpdateBlinkers()
    {
        // Se nenhuma seta estiver ligada, apaga tudo
        if (!leftOn && !rightOn && !hazardOn)
        {
            SetBlinkLights(0f, 0f);
            return;
        }

        // Contador de tempo
        blinkTimer += Time.deltaTime;

        // Alterna ligado/desligado
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            blinkState = !blinkState;
        }

        float value = blinkState ? blinkIntensity : 0f;

        // Pisca-alerta
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

    // Aplica intensidade nas luzes
    void SetBlinkLights(float left, float right)
    {
        if (leftSignal != null)
            leftSignal.intensity = left;

        if (rightSignal != null)
            rightSignal.intensity = right;
    }
}