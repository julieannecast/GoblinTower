using UnityEngine;

public class DefaultProjectileBehavior : MonoBehaviour, IPoolable
{
    public ObjectPoolComponent associatedPool { get; set; }

    private void OnDisable()
    {
        associatedPool.PutObject(gameObject);
    }
}
