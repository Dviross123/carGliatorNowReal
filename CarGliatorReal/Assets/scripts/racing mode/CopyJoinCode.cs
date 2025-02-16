using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CopyJoinCode : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject tempPointer;

    public void OnPointerClick(PointerEventData eventData)
    {
        CopyToClipboard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.visible = false;
        tempPointer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.visible = true;
        tempPointer.SetActive(false);
    }

    void CopyToClipboard()
    {
        string code = text.text.Substring(text.text.Length - 6); // Get last 6 characters
        GUIUtility.systemCopyBuffer = code;
    }
}
