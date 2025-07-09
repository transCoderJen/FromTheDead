using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterMenu : MonoBehaviour
{
    [SerializeField] private GameObject statScreen;
    [SerializeField] private GameObject equipementScreen;

    public void OnStatButtonPressed()
    {
        statScreen.SetActive(true);
        equipementScreen.SetActive(false);
    }
    
    public void OnEquipmentButtonPressed()
    {
        statScreen.SetActive(false);
        equipementScreen.SetActive(true);
    }
}
