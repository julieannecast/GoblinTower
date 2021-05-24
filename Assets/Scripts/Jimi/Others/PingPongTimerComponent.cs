using UnityEngine;

/*
 * Note: Utile pour les animations bouclées.
 */

public class PingPongTimerComponent : MonoBehaviour
{
    [SerializeField] float clockSpeed;

    private float time;
    private float factor;
    public float Time { get; private set; }

    private void Update()
    {
        factor = Mathf.Max(1, factor - UnityEngine.Time.deltaTime);
        Time = Mathf.PingPong(time * clockSpeed, 1f);
        time += UnityEngine.Time.deltaTime / factor;
    }

    public void Slower(float amount)
    {
        factor = amount;
    }

    public void Faster(float amount)
    {
        factor = 1 / amount;
    }
}
