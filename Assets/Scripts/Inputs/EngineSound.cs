using UnityEngine;

public class EngineSound : MonoBehaviour
{
    public AudioSource engineAudio;

    public float minPitch = 0.8f;
    public float maxPitch = 2f;

    void Update()
    {
        float acelerar = Input.GetAxis("Vertical");

        float pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(acelerar));

        engineAudio.pitch = pitch;
    }
}