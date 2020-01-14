using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroAnimController : MonoBehaviour
{
    public OutroVictoryAnim anim;

    public SpriteRenderer[] frogSprites;

    public void SwitchToCloseup()
    {
        closeUp.SetActive(true);
    }

    public void StopFrogAnimating()
    {
        anim.stopAnimating = true;
    }

    public GameObject closeUp;

    // Use this for initialization
    void Start()
    {
        foreach (var sp in frogSprites)
        {
            sp.color = GameController.overallWinnerColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
