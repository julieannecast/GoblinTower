using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateComponent : MonoBehaviour
{
    private int NivReussis;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
