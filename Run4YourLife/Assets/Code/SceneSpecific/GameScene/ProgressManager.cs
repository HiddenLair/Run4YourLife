using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.UI;

public class ProgressManager : MonoBehaviour
{
    [SerializeField]
    private Transform startPhase1;

    [SerializeField]
    private Transform endPhase1;

    [SerializeField]
    private Transform startPhase3;

    [SerializeField]
    private Transform endPhase3;

    private float currentStartX;

    private float currentEndX;

    private GameObject uiManager;

    private GameObject boss;

    private bool bossInsidePoints = false;

    void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag(Tags.UI);

        FindBoss();
    }

    //TODO: FIX ME
    /*void Update()
    {
        if(boss == null || !boss.activeInHierarchy)
        {
            // The boss will be destroyed once a phase is ended
            // Find the new boss if needed

            FindBoss();
        }

        if(boss != null)
        {
            // Find current range

            UpdateStartEndPoints();

            // Current range found

            if(bossInsidePoints)
            {
                ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnBossProgress(ComputeProgress()));
            }
        }
    }*/

    private float ComputeProgress()
    {
        float bossX = boss.transform.position.x;

        return (bossX - currentStartX) / (currentEndX - currentStartX);
    }

    private void FindBoss()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag(Tags.Boss);

        foreach(GameObject currentBoss in bosses)
        {
            if(currentBoss.activeInHierarchy)
            {
                boss = currentBoss;
                break;
            }
        }
    }

    private void UpdateStartEndPoints()
    {
        float bossX = boss.transform.position.x;

        bossInsidePoints = false;

        if(bossX >= startPhase1.position.x && bossX < endPhase1.position.x)
        {
            currentStartX = startPhase1.position.x;
            currentEndX = endPhase1.position.x;

            bossInsidePoints = true;
        }
        else if(bossX >= startPhase3.position.x && bossX < endPhase3.position.x)
        {
            currentStartX = startPhase3.position.x;
            currentEndX = endPhase3.position.x;

            bossInsidePoints = true;
        }
    }
}