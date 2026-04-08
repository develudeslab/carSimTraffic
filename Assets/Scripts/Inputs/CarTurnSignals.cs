using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// Sistema de setas do carro
// Controla:
// - Seta esquerda
// - Seta direita
// - Pisca-alerta
// Permite várias luzes por lado (frente, trás, retrovisor)

public class CarTurnSignalSystem : MonoBehaviour
{
    [Header("Blink Lights")]
    // Lista com TODAS as luzes da seta esquerda
    public List<Light> leftSignals;

    // Lista com TODAS as luzes da seta direita
    public List<Light> rightSignals;

    [Header("Blink Settings")]
    // Intensidade da luz quando está ligada
    public float blinkIntensity = 4f;

    // Tempo entre ligar/desligar (velocidade do pisca)
    public float blinkInterval = 0.5f;

    // Estados das setas
    private bool leftOn = false;   // Seta esquerda ligada?
    private bool rightOn = false;  // Seta direita ligada?
    private bool hazardOn = false; // Pisca-alerta ligado?

    // Controle do tempo do pisca
    private float blinkTimer = 0f;

    // Estado atual do pisca (ligado/desligado)
    private bool blinkState = false;

    // ===== INPUT EVENTS =====
    // Essas funções são chamadas pelo PlayerInput (Unity Events)

    // Seta esquerda (tecla Q)
    public void OnLeftSignal(InputAction.CallbackContext context)
    {
        // Só executa quando o botão é pressionado
        if (context.performed)
        {
            // Alterna estado da seta esquerda
            leftOn = !leftOn;

            // Se ligou a esquerda, desliga direita e alerta
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
            // Alterna estado da seta direita
            rightOn = !rightOn;

            // Se ligou a direita, desliga esquerda e alerta
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
            // Alterna estado do alerta
            hazardOn = !hazardOn;

            // Se ligou alerta, desliga setas individuais
            if (hazardOn)
            {
                leftOn = false;
                rightOn = false;
            }
        }
    }

    // ===== UPDATE =====
    // Chamado a cada frame
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

        // Incrementa o tempo
        blinkTimer += Time.deltaTime;

        // Quando passa o intervalo, alterna o estado (liga/desliga)
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            blinkState = !blinkState;
        }

        // Se blinkState for true, luz ligada
        // Se false, luz desligada
        float value = blinkState ? blinkIntensity : 0f;

        // Se pisca-alerta estiver ligado, ambas piscam
        if (hazardOn)
        {
            SetBlinkLights(value, value);
        }
        else
        {
            // Caso contrário, só o lado ligado pisca
            float leftValue = leftOn ? value : 0f;
            float rightValue = rightOn ? value : 0f;
            SetBlinkLights(leftValue, rightValue);
        }
    }

    // Aplica intensidade nas luzes
    void SetBlinkLights(float left, float right)
    {
        // Percorre todas as luzes da esquerda
        foreach (Light l in leftSignals)
        {
            if (l != null)
                l.intensity = left;
        }

        // Percorre todas as luzes da direita
        foreach (Light l in rightSignals)
        {
            if (l != null)
                l.intensity = right;
        }
    }
}