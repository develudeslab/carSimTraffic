using UnityEngine;

/// <summary>
/// Deve ser colocado em cada objeto que contém o WheelCollider.
/// </summary>
public class WheelVisualSync : MonoBehaviour
{
    [SerializeField] private Transform wheelMesh; // O cilindro visual
    private WheelCollider _collider;

    private void Awake() => _collider = GetComponent<WheelCollider>();

    private void Update()
    {
        Vector3 pos;
        Quaternion rot;
        
        // Pega a posição real calculada pela física da Unity 
        _collider.GetWorldPose(out pos, out rot);
        
        // Aplica ao objeto visual 
        wheelMesh.position = pos;
        wheelMesh.rotation = rot;
    }
}
