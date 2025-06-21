using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class GloryKillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] List<KeyCode> keyCodeList;
    private GameObject createdHotkey;
    private bool hotkeyPressed = false;

    public void HotkeyPressed(bool activate)
    {
        hotkeyPressed = activate;
    }

    public bool WasHotkeyPressed()
    {
        return hotkeyPressed;
    }

    public void CreateHoykey(Enemy enemy)
    {
        if (keyCodeList.Count <= 0)
            return;

        Vector3 proposedPosition = enemy.transform.position + new Vector3(0, 2);
        proposedPosition = GetValidPosition(proposedPosition);

        createdHotkey = Instantiate(hotkeyPrefab, proposedPosition, Quaternion.identity);

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);

        EnemyHotkeyController newHotkeyScript = createdHotkey.GetComponent<EnemyHotkeyController>();
        newHotkeyScript.SetupHotkey(chosenKey, enemy.transform, this);
    }

    public void DestroyHotHeys()
    {
        Destroy(createdHotkey);
    }

    private Vector3 GetValidPosition(Vector3 initialPosition)
    {
        Vector3 offset = new Vector3(1, 0, 0); // Adjust this offset as needed to position the hotkey next to the previous one.
        Vector3 checkPosition = initialPosition;

        // while (IsPositionOccupied(checkPosition))
        // {
        //     checkPosition += offset;
        // }

        return checkPosition;
    }
}