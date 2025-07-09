using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public string checkpointName;
    public bool activated;
    [SerializeField] private Material checkpointMat;
    private PulseIntensity pulseIntensity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        pulseIntensity = GetComponent<PulseIntensity>();
        
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
        AudioManager.Instance.PlaySFX("Checkpoint");
        PlayerManager.Instance.player.respawnPosition = transform;
        PlayerManager.Instance.player.stats.IncreaseHealthBy(1000);
        PlayerManager.Instance.player.GeneratePulse();
        activated = true;
        GetComponent<SpriteRenderer>().material = checkpointMat;
        anim.SetBool("Activating", true);
        pulseIntensity.enabled = true;
    }

    private void ActivateFinishTrigger()
    {
        anim.SetBool("Activating", false);
        anim.SetBool("Activated", true);
    }
}
