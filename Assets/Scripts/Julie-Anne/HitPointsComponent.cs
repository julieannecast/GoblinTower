using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointsComponent : MonoBehaviour
{
    public int VieMax;
    private int VieCourante;

    private void Awake()
    {
        VieCourante = VieMax;
    }

    public void MangerDommage(int dommage)
    {
        VieCourante -= dommage;
    }
    
}
