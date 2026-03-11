using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMassSetter : MonoBehaviour
{
    [SerializeField] private Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0); 

    private void Start()
    {
        // Corrigido: Atribui o offset ao centro de massa do Rigidbody 
        GetComponent<Rigidbody>().centerOfMass = centerOfMassOffset;
    }
}
