using UnityEngine;

public class DefaultProjectileBehavior : MonoBehaviour, IPoolable
{
    public ObjectPoolComponent associatedPool { get; set; }
    [SerializeField] int damage;

    private void OnDisable()
    {
        associatedPool.PutObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HitPointsComponent>().TakeDamage(damage);
        }
    }
}
