using UnityEngine;
using Photon.Pun;
public class SnappedWindmillRepair : MonoBehaviourPunCallbacks
{

    [SerializeField] private Animator anim;
    private Windmill owner;
    public string highlightText = "[F]";
    private AudioSource tree;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        owner = GetComponentInParent<Windmill>();
        tree = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC] 
    private void RPCRepairWindmill()
    {
        
        anim.SetTrigger("EarthRepair");
        tree.Play();
        owner.ReadyToSpin();
    }
    private void OnTriggerStay(Collider other)
    {
        if (owner.currentState == WindmillDamageState.snapped && other.TryGetComponent(out Player player))
        {
            PlayerUI.main.UpdateHighlightText(highlightText, transform.position, 1);
            if (Input.GetKey(KeyCode.F) && player.currentType == Player.PlayerType.Earth)
            {
                photonView.RPC("RPCRepairWindmill", RpcTarget.AllBuffered);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            PlayerUI.main.UpdateHighlightText("", Vector3.zero, 0f);
        }
    }
}
