using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenerUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float moveDuration = 0.5f; // Time for animation
    [SerializeField] float marginToStay = 100f;
    [SerializeField] AnimationCurve movementCurve; // Add this in Inspector

    private RectTransform rect;
    private bool animating;
    private float elapsedTime;
    private Vector2 startPos, targetPos;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        float startX = -rect.rect.width + marginToStay;
        startPos = new Vector2(startX, rect.position.y);
        targetPos = new Vector2(0, rect.position.y);
        rect.position = startPos; // Start hidden
    }

    private void Update()
    {
        if (animating)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration); // Normalize time
            float curveValue = movementCurve.Evaluate(t); // Apply curve

            rect.position = Vector2.Lerp(startPos, targetPos, curveValue);

            if (t >= 1f) animating = false; // Stop animation
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartAnimation(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartAnimation(false);
    }

    private void StartAnimation(bool opening)
    {
        animating = true;
        elapsedTime = 0f;
        startPos = rect.position;
        targetPos = opening ? new Vector2(0, rect.position.y) : new Vector2(-rect.rect.width + marginToStay, rect.position.y);
    }
}
