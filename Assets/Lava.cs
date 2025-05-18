using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Lava : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            GetComponent<CharacterStats>().DoMagicDamage(player.stats, false);
            player.stateMachine.ChangeState(player.respawnHolyState);
            
        }
    }
}
