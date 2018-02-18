using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class CutPlane : MonoBehaviour {

    public SlicedHull SliceObject(GameObject obj)
    {
        EzySlice.Plane cuttingPlane = ComputePlaneAgainst(obj);

        // finally, slice the object and return the results. SlicedHull will have all the mesh
        // details which the application can use to do whatever it wants to do
        return Slicer.Slice(obj, cuttingPlane);
    }

    /**
     * Computes a Plane in regards to the reference frame of the provided GameObject
     * which can be used to cut the provided Object
     */
    public EzySlice.Plane ComputePlaneAgainst(GameObject obj)
    {
        // ensure to generate an EzySlice version of the Plane instead of the 
        // default Unity.
        EzySlice.Plane cuttingPlane = new EzySlice.Plane();

        // since this GameObject represents our Plane's coordinates, we first need
        // to bring the Plane into the coordinate frame of the object we want to slice
        // this is because the Mesh data is always in local coordinates
        // we need the position of the plane and direction
        Vector3 refUp = obj.transform.InverseTransformDirection(transform.up);
        Vector3 refPt = obj.transform.InverseTransformPoint(transform.position);

        // once we have the coordinates we need, we can initialize our plane with the new
        // coordinates (now in obj's coordinate frame) and safely perform the slice
        // operation
        cuttingPlane.Compute(refPt, refUp);

        return cuttingPlane;
    }
}
