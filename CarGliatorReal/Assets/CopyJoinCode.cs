using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CopyJoinCode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = text.text;
    }
}
