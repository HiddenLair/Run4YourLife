using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGoThroughManager : SingletonMonoBehaviour<PlatformGoThroughManager> {

    private HashSet<GameObject> m_ignoredRunners = new HashSet<GameObject>();
    public HashSet<GameObject> IgnoredRunners { get { return m_ignoredRunners; } }

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private void Start()
    {
        StartCoroutine(ClearRunnersAtEndOfFrame());
    }

    private IEnumerator ClearRunnersAtEndOfFrame()
    {
        while(true)
        {
            yield return waitForEndOfFrame;
            m_ignoredRunners.Clear();
        }
    }

    public void IgnoreCollision(GameObject runner)
    {
        m_ignoredRunners.Add(runner);
    }
}
