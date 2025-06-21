using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.playBGM = false;
        AudioManager.Instance.PlayBossMusic();
    }
}
