using UnityEngine;

public class CreditsTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        }
    }
}
