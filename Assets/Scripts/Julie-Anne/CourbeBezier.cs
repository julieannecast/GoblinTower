using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public struct CourbeBezier
{
    private Vector3 pZero;
    private Vector3 pUn;
    private Vector3 pDeux;

    public CourbeBezier(Vector3 zero, Vector3 un, Vector3 deux)
    {
        pZero = zero;
        pUn = un;
        pDeux = deux;
    }
    public Vector3 Evaluer(float t)
    {
        float a = (1 - t)*(1 - t);
        float b = 2*(1-t)*t;
        float c = t*t;

        return a * pZero + b * pUn + c * pDeux;
    }

    public static Vector3 Evaluer(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float a = (1 - t) * (1 - t);
        float b = 2 * (1 - t) * t;
        float c = t * t;

        return a * p1 + b * p2 + c * p3;
    }
}

