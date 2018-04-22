using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife;

public class TrapInit : MonoBehaviour {

    #region Inspector
    [SerializeField]
    private float fadeInTime;
    #endregion
    #region Variables

    private bool grounded =false;
    private bool faded = false;

    #endregion

    // Use this for initialization
    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
        Color actualC = GetComponentInChildren<Renderer>().material.color;
        actualC.a = 0;
        GetComponentInChildren<Renderer>().material.color = actualC;
        StartCoroutine(FadeIn(fadeInTime));
    }

    // Update is called once per frame
    void Update () {
		if(faded && grounded)
        {
            GetComponent<Collider>().enabled = true;
            Destroy(this);
        }
	}

    IEnumerator Fall()
    {
        GetComponent<Rigidbody>().useGravity = true;
        while (!grounded)
        {
            RaycastHit info;
            if (Physics.Raycast(transform.position, Vector3.down,out info, 0.5f, Layers.Stage, QueryTriggerInteraction.Ignore))
            {
                transform.position = transform.position + Vector3.down * info.distance;
                grounded = true;
                Destroy(GetComponent<Rigidbody>());
                //Set ground as parent
                transform.SetParent(info.collider.gameObject.transform);
            }
            yield return 0;
        }
    }

    IEnumerator FadeIn(float delay)
    {
        //TODO: FIX THIS DO NOT USE CONSTATNT Time.deltaTime
        float fps = 1 / Time.deltaTime;
        float alphaPerFrame = 1 / (delay * fps);
        Color temp = GetComponentInChildren<Renderer>().material.color;
        while (temp.a < 1)
        {
            temp.a += alphaPerFrame;
            GetComponentInChildren<Renderer>().material.color = temp;
            yield return 0;
        }
        faded = true;

        SetToOpaque(gameObject.GetComponentInChildren<Renderer>().material);
        StartCoroutine(Fall());
    }

    public void SetToOpaque(Material material)
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
}
