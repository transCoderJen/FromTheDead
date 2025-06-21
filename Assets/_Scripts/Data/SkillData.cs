using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite skillSprite;
    public GameObject skillPrefab;
    [TextArea(3, 10)]
    public string description;
    public float cooldown;
}
