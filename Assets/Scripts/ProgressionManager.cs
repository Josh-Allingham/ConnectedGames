using UnityEngine;
using Photon.Pun;
public class ProgressionManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static ProgressionManager main;
    [SerializeField] private ProgressionState currentState = ProgressionState.PreOldMan;
    [SerializeField] private Windmill[] windmills = new Windmill[3];
    [SerializeField] private GameObject Rockbridge;
    [SerializeField] private Cauldron cauldron;
    [SerializeField] private Animator GateAnimator;
    [SerializeField] private GameObject oldMan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main = this;
        GateAnimator.SetBool("GateOpen", false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ProgressionState.PreOldMan:
                //once spoken to, pan away and vanish
                break;
            case ProgressionState.PostOldMan:
                bool allWindmillsSpinning = true;
                foreach (Windmill w in windmills)
                {
                    allWindmillsSpinning &= w.isSpinning;
                }
                Debug.Log(allWindmillsSpinning + "spin");
                if (allWindmillsSpinning)
                {
                    //Pan to gate
                    CameraManager.main.ActivateCamera("GateCam");
                    StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("GateCam", 2));
                    GateAnimator.SetBool("GateOpen", true);
                    ChangeState(ProgressionState.WindmillsActive);
                }
                break;

            case ProgressionState.WindmillsActive:
                 if (cauldron.IsActive())
                {
                    // spawn bridge to boss
                    Rockbridge.GetComponent<Animator>().SetTrigger("RaiseBridge");
                    CameraManager.main.ActivateCamera("Bridge");
                    ChangeState(ProgressionState.PlatformsSpawned);
                    StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("Bridge", 4));
                    oldMan.GetComponentInChildren<NPC>().PrepareForBattle();
                }
                break;
            case ProgressionState.PlatformsSpawned:
                break;
        }
    }
    public void ChangeState(ProgressionState state)
    {
        photonView.RPC("RPCChangeState", RpcTarget.All, state);
    }

    [PunRPC] public void RPCChangeState(ProgressionState state)
    {
        currentState = state;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    public enum ProgressionState
    {
        PreOldMan,
        PostOldMan,
        WindmillsActive,
        PlatformsSpawned
    }
}
