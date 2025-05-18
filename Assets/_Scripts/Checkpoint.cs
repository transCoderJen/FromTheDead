using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activated;
    [SerializeField] private Material checkpointMat;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !activated)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        // TODO Checkpint SFX
        // AudioManager.Instance.PlaySFX(SFXSounds.checkpoint, null);
        PlayerManager.Instance.player.respawnPosition = transform;
        activated = true;
        GetComponent<SpriteRenderer>().material = checkpointMat;
        anim.SetBool("Activating", true);
    }

    private void ActivateFinishTrigger()
    {
        anim.SetBool("Activating", false);
        anim.SetBool("Activated", true);
    }
}
