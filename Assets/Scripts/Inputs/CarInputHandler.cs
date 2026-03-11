using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centraliza a leitura de comandos do Input System.
/// </summary>
public class CarInputHandler : MonoBehaviour
{
    private CarInputActions _inputActions;
    
    // Propriedades públicas apenas para leitura (Read-only)
    public Vector2 MoveInput { get; private set; }
    public bool IsBraking { get; private set; }

    private void Awake()
    {
        _inputActions = new CarInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Driving.Enable();
        
        // Inscrição nos eventos
        _inputActions.Driving.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _inputActions.Driving.Move.canceled += ctx => MoveInput = Vector2.zero;
        
        _inputActions.Driving.Brake.performed += ctx => IsBraking = true;
        _inputActions.Driving.Brake.canceled += ctx => IsBraking = false;
    }

    private void OnDisable()
    {
        _inputActions.Driving.Disable();
    }
}
