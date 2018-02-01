using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TestMySpline;

public class SplineEditor : MonoBehaviour {

    public Transform[] positions;

    public Transform[] p;

    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawBezier(p[0].position, p[1].position, p[2].position, p[3].position, Color.white, null, 2f);
        float[] x = new float[positions.Length];
        float[] y = new float[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            x[i] = positions[i].position.x;
            y[i] = positions[i].position.y;
        }

        int nEvaluations = 30;
        /*float[] xs = new float[nEvaluations];
        for (int i = 0; i < nEvaluations; i++)
        {
            xs[i] = points[0].x + (points[points.Length - 1].x - points[0].x) * ((float)i / (nEvaluations-1));
        }*/

        float[] xs;
        float[] ys;
        CubicSpline.FitParametric(x, y, nEvaluations, out xs, out ys);

        Vector3[] res = new Vector3[xs.Length];
        for (int i = 0; i < xs.Length; i++)
        {
            res[i].x = xs[i];
            res[i].y = ys[i];
        }

        for (int i = 0; i < res.Length-1; i++)
        {
            Gizmos.DrawLine(res[i], res[i + 1]);
        }
    }
}
