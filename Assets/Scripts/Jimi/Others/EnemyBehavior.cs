using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] Transform exit;
    [SerializeField] float cooldown;
    [SerializeField] ObjectPoolComponent bulletObjectPool;
    [SerializeField] Transform target;
    [SerializeField] float aggroRange;

    private bool isShooting;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        transform.LookAt(target);

        isShooting = TargetIsClose(target.position);
    }

    IEnumerator Spawn() 
    {
        while (true)
        {
            if (isShooting) 
            {
                try
                {
                    var recycledBullet = bulletObjectPool.GetObject();
                    ResetBullet(recycledBullet);
                } catch { }
            }

            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private bool TargetIsClose(Vector3 target) =>
        Vector3.Distance(transform.position, target) <= aggroRange;

    private void ResetBullet(GameObject recycledBullet)
    {
        recycledBullet.transform.position = exit.position;
        recycledBullet.transform.rotation = exit.rotation;
        recycledBullet.SetActive(true);
    }
}
