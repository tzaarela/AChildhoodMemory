using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] TMP_Text myText = null;

    public string sequenceName;

    IEnumerator Start()
    {
        myText.maxVisibleCharacters = 0;
        yield return new WaitForSeconds(6.5f);
        StartCoroutine(RevealText(myText));
        yield return new WaitForSeconds(11f);
        ChangeScene();
    }

    private static IEnumerator RevealText(TMP_Text text)
    {
        int totalVisibleCharacters = text.textInfo.characterCount;
        int counter = 0;
        while (counter < totalVisibleCharacters + 1)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            text.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(0.06f);
            counter += 1;
        }
        yield return new WaitForSeconds(2);
        text.maxVisibleCharacters = 0;
    }

    public void ChangeScene()
    {
        if(sequenceName == "Intro")
            SceneController.Instance.ChangeScene("GameScene");
        if(sequenceName == "Outro")
            SceneController.Instance.ChangeScene("MainMenu");
    }
}
