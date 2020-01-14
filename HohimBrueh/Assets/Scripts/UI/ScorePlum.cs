using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePlum : MonoBehaviour
{
    float displayTimeLeft;
    
    public Text text;
    
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Character>().player == null)
            return;

        if (displayTimeLeft > 0f)
        {
            displayTimeLeft -= Time.deltaTime;
            if (Time.time % 0.15f < 0.05f)
                text.color = GetComponent<Character>().player.color;
            else if (Time.time % 0.15f < 0.1f)
                text.color = Color.white;
            else text.color = Color.black;

            if (displayTimeLeft > 1.5f)
            {
                text.transform.localScale = Vector3.one * Mathf.Clamp ((1f +  (displayTimeLeft - 1.5f) * 2f),1f,1.5f);
            }
            else
            {
                text.transform.localScale = Vector3.one;
            }

            if (displayTimeLeft <= 0f)
            {
                text.text = "";
            }
        }
    }

    public void ShowText(string text, float time = 2f)
    {
        this.text.text = text;
        displayTimeLeft = time;
    }
}
