using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameManager : NetworkBehaviour
{
    [SerializeField]TextMeshProUGUI CountDownText;
    public IEnumerator CountDown()
    {
        CountDownText.text = "Starts In: 3";
        yield return new WaitForSeconds(1);
        CountDownText.text = "Starts In: 2";
        yield return new WaitForSeconds(1);
        CountDownText.text = "Starts In: 1";
        yield return new WaitForSeconds(1);
        CountDownText.text = "GO!";
    }

    private void Start()
    {
    //   StartCoroutine(CountDown());
    }
}
