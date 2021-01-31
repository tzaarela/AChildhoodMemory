using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour
{
    [SerializeField] TMP_Text myText = null;

    IEnumerator Start()
    {
        myText.maxVisibleCharacters = 0;
        yield return new WaitForSeconds(5.5f);
        StartCoroutine(RevealText(myText));
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
}
