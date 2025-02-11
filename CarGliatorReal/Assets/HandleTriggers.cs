using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HandleTriggers : MonoBehaviour
{

    [Header("Laps")]
    [SerializeField] int CurrentLap = 0;
    [SerializeField] int MaxLaps = 3;
    [SerializeField] TextMeshProUGUI LapsText;

    private void Start()
    {
        CurrentLap = 0;
        UpdateLapText();
    }
    public void UpdateLapText()
    {      
        LapsText.text = "Lap: " + CurrentLap + "/" + MaxLaps;
    }

    public void UpdateLapValue()
    {
        if (CurrentLap < MaxLaps)
        {
            CurrentLap++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            UpdateLapValue();
            UpdateLapText();
        }
       
    }
}
