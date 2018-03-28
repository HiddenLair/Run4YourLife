using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

using Run4YourLife.Utils;

public class CutTrigger : MonoBehaviour {

    CutPlane cuts;
    public Material crossMat;
    public new GameObject particleSystem;
    public float cutTime = 1.5f;
    public float delayedDestroyTime = 2.0f;
    private float timer = 0.0f;
    private GameObject g=null;

    private void Awake()
    {
        cuts = GetComponentInChildren<CutPlane>();
    }

    void Update()
    {
        if (g != null)
        {
            if (timer >= cutTime)
            {
                if (g.transform.childCount == 0)
                {
                    SlicedHull hull = cuts.SliceObject(g);

                    if (hull != null)
                    {
                        GameObject g1 = hull.CreateLowerHull(g, crossMat);
                        GameObject g2 = hull.CreateUpperHull(g, crossMat);
                        if (g.transform.parent != null)
                        {
                            if (g1 != null)
                            {
                                g1.transform.SetParent(g.transform.parent, false);
                            }
                            if (g2 != null)
                            {
                                g2.transform.SetParent(g.transform.parent, false);
                            }
                        }

                        GameObject particle = Instantiate(particleSystem, g1.transform.position, g1.transform.rotation);
                        ParticleSystem particleComponent = particle.GetComponent<ParticleSystem>();
                        var shape = particleComponent.shape;
                        shape.shapeType = ParticleSystemShapeType.Mesh;
                        shape.mesh = g1.GetComponent<MeshFilter>().mesh;
                        Destroy(g1);
                        // StartCoroutine(DestroyDelayed(particle));
                        StartCoroutine(YieldHelper.WaitForSeconds(Destroy, particle, delayedDestroyTime));


                        Destroy(g);
                        g = g2;
                    }
                }
                else
                {

                }
                timer = 0.0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        g = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        g = null;
    }

    /* IEnumerator DestroyDelayed(GameObject g)
    {
        yield return new WaitForSeconds(delayedDestroyTime);
        Destroy(g);
    } */

    public GameObject[] SliceObjectRecursive(CutPlane plane, GameObject obj)
    {

        // finally slice the requested object and return
        SlicedHull finalHull = plane.SliceObject(obj);

        if (finalHull != null)
        {
            GameObject lowerParent = finalHull.CreateLowerHull(obj, crossMat);
            GameObject upperParent = finalHull.CreateUpperHull(obj, crossMat);

            if (obj.transform.childCount > 0)
            {
                foreach (Transform child in obj.transform)
                {
                    if (child != null && child.gameObject != null)
                    {

                        // if the child has chilren, we need to recurse deeper
                        if (child.childCount > 0)
                        {
                            GameObject[] children = SliceObjectRecursive(plane, child.gameObject);

                            if (children != null)
                            {
                                // add the lower hull of the child if available
                                if (children[0] != null && lowerParent != null)
                                {
                                    children[0].transform.SetParent(lowerParent.transform, false);
                                }

                                // add the upper hull of this child if available
                                if (children[1] != null && upperParent != null)
                                {
                                    children[1].transform.SetParent(upperParent.transform, false);
                                }
                            }
                        }
                        else
                        {
                            // otherwise, just slice the child object
                            SlicedHull hull = plane.SliceObject(child.gameObject);

                            if (hull != null)
                            {
                                GameObject childLowerHull = hull.CreateLowerHull(child.gameObject, crossMat);
                                GameObject childUpperHull = hull.CreateUpperHull(child.gameObject, crossMat);

                                // add the lower hull of the child if available
                                if (childLowerHull != null && lowerParent != null)
                                {
                                    childLowerHull.transform.SetParent(lowerParent.transform, false);
                                }

                                // add the upper hull of the child if available
                                if (childUpperHull != null && upperParent != null)
                                {
                                    childUpperHull.transform.SetParent(upperParent.transform, false);
                                }
                            }
                        }
                    }
                }
            }

            return new GameObject[] { lowerParent, upperParent };
        }

        return null;
    }
}
