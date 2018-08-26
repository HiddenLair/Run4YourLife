using UnityEngine;

using Run4YourLife.GameManagement;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject background1;

    [SerializeField]
    private GameObject background2;

    [SerializeField]
    private GameObject background3;

    private GameObject[] backgrounds = new GameObject[3];

    void Awake()
    {
        backgrounds[0] = background1;
        backgrounds[1] = background2;
        backgrounds[2] = background3;

        GameManager.Instance.onGamePhaseChanged.AddListener(OnGamePhaseChanged);
    }

    private void OnGamePhaseChanged(GamePhase gamePhase)
    {
        switch(gamePhase)
        {
            case GamePhase.TransitionPhase1Start:
            case GamePhase.EasyMoveHorizontal:
                UpdateBackground(0);
                break;
            case GamePhase.TransitionPhase2Start:
            case GamePhase.BossFight:
                UpdateBackground(1);
                break;
            case GamePhase.TransitionPhase3Start:
            case GamePhase.HardMoveHorizontal:
                UpdateBackground(2);
                break;
        }
    }

    private void UpdateBackground(int index)
    {
        foreach(GameObject background in backgrounds)
        {
            background.SetActive(false);
        }

        backgrounds[index].SetActive(true);
    }
}