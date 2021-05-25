using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/*
 * Note: 'CallbackOnEmit' offre la possibilité d'appeler une fonction lors d'une emission.
 *        Exemple: Fonction de recoil, de particules, etc...
 *        
 */

public class ObjectEmitterComponent : MonoBehaviour
{
    public Transform exitPoint;
    [SerializeField] float cooldown;
    [SerializeField] ObjectPoolComponent objectPool;
    [SerializeField] UnityEvent<GameObject> CallBackOnEmit;

    private bool isEmitting;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isEmitting = false;
        StartCoroutine(Emit());
    }

    public void CanEmit(bool canEmit)
    {
        isEmitting = canEmit;
    }

    IEnumerator Emit()
    {
        while (true)
        {
            if (isEmitting)
            {
                try
                {
                    var recycledObject = objectPool.GetObject();
                    CallBackOnEmit.Invoke(recycledObject);
                    ResetObject(recycledObject);
                }
                catch { }
            }

            yield return new WaitForSecondsRealtime(cooldown);
        }
    }

    private void ResetObject(GameObject recycledObject)
    {
        recycledObject.transform.position = exitPoint.position;
        recycledObject.transform.rotation = exitPoint.rotation;
        recycledObject.SetActive(true);
    }
}
