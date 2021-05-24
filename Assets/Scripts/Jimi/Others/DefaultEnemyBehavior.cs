using UnityEngine;

public class DefaultEnemyBehavior : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float aggroRange;
    [SerializeField] ObjectEmitterComponent emitter;

    private void Update()
    {
        transform.LookAt(target);
        emitter.CanEmit(TargetIsCloseEnough(target.position));
    }

    private bool TargetIsCloseEnough(Vector3 target) =>
        Vector3.Distance(transform.position, target) <= aggroRange;
}
