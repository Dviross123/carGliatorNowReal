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
    public CheckPointClass[] CheckPointsArr;

    [Header("Physics")]
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CurrentLap = 0;
        UpdateLapText();
        SetCheckPointsArr();
      
    }

    void SetCheckPointsArr()
    {
        Transform CheckPointTrans = GameObject.Find("CheckPoints").transform;
        CheckPointsArr = new CheckPointClass[CheckPointTrans.childCount - 1];

        for (int i = 0; i < CheckPointsArr.Length; i++)
        {
            CheckPointsArr[i] = new CheckPointClass(); 
            CheckPointsArr[i].builder();
            CheckPointsArr[i].bc = CheckPointTrans.GetChild(i).GetComponent<BoxCollider>();
        }

        foreach (CheckPointClass cpc in CheckPointsArr)
        {
            cpc.archived = true;
        }
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

    //continue checkPoints
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            foreach(CheckPointClass cpc in CheckPointsArr)
            {
                if (cpc.bc == other.GetComponent<BoxCollider>() && cpc.archived == false)
                {
                    cpc.archived = true;
                }
            }
        }
        else if (other.CompareTag("FlagPoint"))
        {
            bool allCheckPointsArchived = true;
            foreach (CheckPointClass cpc in CheckPointsArr)
            {
                if (cpc.archived == false)
                {
                    allCheckPointsArchived = false;
                    break;
                }
            }

            if (allCheckPointsArchived)
            {
                UpdateLapValue();
                UpdateLapText();

                foreach (CheckPointClass cpc in CheckPointsArr)
                {
                  cpc.archived = false;                
                }
            }
           
        }
        else if (other.CompareTag("SpeedBooster"))
        {
            rb.AddForce(other.transform.forward.normalized * other.GetComponent<SpeedBooster>().amount, ForceMode.Impulse);
        }
       
    }
}
