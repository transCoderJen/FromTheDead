using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public Sprite spellSprite;
    public GameObject spellPrefab;
    [TextArea(3, 10)]
    public string description;
    public float cooldown;
    public string itemId;

       private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
