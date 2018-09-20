using UnityEngine;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;

public class FXReceiver : MonoBehaviour
{
    [SerializeField]
    private GameObject fx;

    [SerializeField]
    private AudioClip sfx;

    public GameObject FX { get { return fx; } }
    public AudioClip SFX { get { return sfx; } }

    public GameObject PlayFx(bool setAsParent = false)
    {
        if (sfx != null)
        {
            AudioManager.Instance.PlaySFX(sfx);
        }

        return FXManager.Instance.InstantiateFromReceiver(this, setAsParent);
    }
}
