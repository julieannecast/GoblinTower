using UnityEngine;

public class SpikyBehavior : MonoBehaviour
{
    [SerializeField] AutoJumpOnFallComponent autoJumpOnFallComponent;
    [SerializeField] PingPongTimerComponent timer;

    private void OnTriggerEnter(Collider other)
    {
        timer.Slower(2f); //Ralentissement temporaire de l'animation
        autoJumpOnFallComponent.Jump(); //Forcer un saut
    }
}