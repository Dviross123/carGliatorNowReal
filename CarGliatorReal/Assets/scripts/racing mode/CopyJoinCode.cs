using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CopyJoinCode : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI text;

    public void OnPointerClick(PointerEventData eventData)
    {
        CopyToClipboard();
    }

    void CopyToClipboard()
    {
      string code = text.text.Substring(text.text.Length - 6); // Get last 6 characters
      GUIUtility.systemCopyBuffer = code;       
    }

   
}
