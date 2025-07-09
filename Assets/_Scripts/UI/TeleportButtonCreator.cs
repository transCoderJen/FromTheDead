using UnityEngine;

public class TeleportButtonCreator : MonoBehaviour
{
    [SerializeField] private GameObject viewport;
    [SerializeField] private Sprite UISprite;

    private void Start()
    {
        foreach (var checkpoint in DebugManager.Instance.checkpoints)
        {
            var cp = checkpoint; // capture for closure
            GameObject buttonObj = new GameObject("TeleportTo_" + cp.name);
            buttonObj.transform.SetParent(viewport.transform, false);

            var button = buttonObj.AddComponent<UnityEngine.UI.Button>();
            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);

            // Add Image component and set its sprite
            var image = buttonObj.AddComponent<UnityEngine.UI.Image>();
            image.sprite = UISprite;
            button.targetGraphic = image;

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            var text = textObj.AddComponent<UnityEngine.UI.Text>();
            text.text = cp.checkpointName;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.alignment = TextAnchor.MiddleCenter;
            text.rectTransform.sizeDelta = rectTransform.sizeDelta;
            text.color = Color.black; // Set text color to black

            button.onClick.AddListener(() =>
            {
                PlayerManager.Instance.player.transform.position = cp.transform.position;
                this.gameObject.SetActive(false); // Hide the teleport button after teleporting
            });
        }
    }
}
