using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Utils;
using Run4YourLife.Player;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RunnerGodModeControlScheme))]
public class RunnerGodModeController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float speedMultiplier = 3.0f;

    private GameObject runner = null;
    private RunnerGodModeControlScheme runnerGodModeControlScheme = null;

    void Awake()
    {
        runnerGodModeControlScheme = GetComponent<RunnerGodModeControlScheme>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateEnd();
    }

    public void Begin(GameObject runner)
    {
        this.runner = runner;
        transform.position = runner.transform.position;

        ExecuteEvents.Execute<IPlayerHandleEvent>(gameObject, null, (x, y) => x.OnPlayerHandleChanged(runner.GetComponent<PlayerInstance>().PlayerHandle));
        StartCoroutine(YieldHelper.SkipFrame(() => { runnerGodModeControlScheme.Active = true; }));
    }

    private void End()
    {
        Destroy(gameObject);
        runner.SetActive(true);
        runner.transform.position = transform.position;
    }

    private void UpdateMovement()
    {
        Vector3 positionInc = Vector3.zero;
        positionInc += runnerGodModeControlScheme.MoveH.Value() * Vector3.right;
        positionInc += runnerGodModeControlScheme.MoveV.Value() * Vector3.up;
        positionInc *= speed * (runnerGodModeControlScheme.MSpeed.Persists() ? speedMultiplier : 1.0f);
        positionInc *= Time.deltaTime;

        transform.position += positionInc;
    }

    private void UpdateEnd()
    {
        if(runnerGodModeControlScheme.End.Started())
        {
            End();
        }
    }
}