using UnityEditor;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public string itemId;
    protected virtual void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif    
    }
}
