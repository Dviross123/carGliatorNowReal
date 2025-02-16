using UnityEngine;
using UnityEngine.UI;

public class UiFollowMouse : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] Vector2 offset;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePosition
        );
        rectTransform.anchoredPosition = mousePosition + offset;
    }
}
