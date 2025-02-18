using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Btn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //[Header("Only Ready Btn")]
    //[SerializeField] Image image;
    //[SerializeField] TextMeshProUGUI text;
    //[SerializeField] Color ReadyColor;
    //[SerializeField] Color CancalReadyColor;

    bool ready = false;
    [Header("Resize Btn")]
    [SerializeField] float btnScaleMultiplaier = 1.1f;
    Vector2 normalBtnScale;
    RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        normalBtnScale = rect.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale *= btnScaleMultiplaier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = normalBtnScale;
    }
    //public void HandleReady()
    //{
    //    if (ready)
    //    {
    //        // make not ready
    //        text.text = "Ready";
    //        image.color = ReadyColor;
    //        ready = false;
            
    //    }
    //    else
    //    {
    //        // make ready
    //        text.text = "Cancal Ready";
    //        image.color = CancalReadyColor;
    //        ready = true;
    //    }
    //}

    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetHost(bool what)
    {
        UserData.isHost = what;
    }

    public void TextToCode(TextMeshProUGUI text)
    {
        UserData.codeEntered =  text.text.Substring(0, 6);
    }

    public void StartGame(TestLobby testLobby)
    {
        testLobby.StartGame();
    }
}
