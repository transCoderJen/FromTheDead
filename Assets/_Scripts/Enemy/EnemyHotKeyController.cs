using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHotkeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    GloryKillController gloryKill;

    public void SetupHotkey(KeyCode _myNewHotkey, Transform _myEnemy, GloryKillController _myGloryKill)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myHotkey = _myNewHotkey;
        myText.text = _myNewHotkey.ToString();
        myEnemy = _myEnemy;
        gloryKill = _myGloryKill;
    }

    private void Update()
    {
        if (myEnemy != null)
        {
            transform.position = GetValidPosition(myEnemy.position + new Vector3(0, 2, 0));
        }
        if (Input.GetKeyDown(myHotkey))
        {
            myText.color = Color.clear;
            sr.color = Color.clear;
            gloryKill.HotkeyPressed(true);
        }
    }

    private Vector3 GetValidPosition(Vector3 initialPosition)
    {
        Vector3 offset = new Vector3(1, 0, 0); // Adjust this offset as needed to position the hotkey next to the previous one.
        Vector3 checkPosition = initialPosition;

        while (IsPositionOccupied(checkPosition))
        {
            checkPosition += offset;
        }

        return checkPosition;
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        // Find all EnemyHotkeyController objects in the scene
        EnemyHotkeyController[] allHotkeys = FindObjectsByType<EnemyHotkeyController>(FindObjectsSortMode.None);

        foreach (var hotkey in allHotkeys)
        {
            // Skip self
            if (hotkey == this) continue;

            if (Vector3.Distance(hotkey.transform.position, position) < 0.5f) // Adjust the distance threshold as needed.
            {
                return true;
            }
        }

        return false;
    }
}
