using UnityEngine;

public class SpikyBehavior : MonoBehaviour
{
    [SerializeField] AutoJumpOnFallComponent autoJumpOnFallComponent;
    [SerializeField] PingPongTimerComponent timer;
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HitPointsComponent>().TakeDamage(damage);
        }
        else
        {
            if (timer) timer.Slower(2f); //Ralentissement temporaire de l'animation
            if (autoJumpOnFallComponent) autoJumpOnFallComponent.Jump(); //Forcer un saut
        }
    }
}