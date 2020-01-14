using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenController : MonoBehaviour
{
    public PlayerScoreDisplay scoreDisplayPrefab;

    public Canvas scoreCanvas;

    public Text roundText;

    bool isLastRound
    {
        get
        {
            return GameController.levelNo >= GameController.levelNames.Length;
        }
    }


    List<PlayerScoreDisplay> playerScoreDisplays;

    PlayerScoreDisplay redScoreDisplay;
    PlayerScoreDisplay blueScoreDisplay;

    float updateScoresDelay = 1f;
    float continueDelay = 5f;

    // Use this for initialization
    void Awake()
    {
        roundText.text = "ROUND " + GameController.levelNo + " OF " + GameController.levelNames.Length;
        playerScoreDisplays = new List<PlayerScoreDisplay>();

        if (GameController.isShowDown)
            updateScoresDelay = 0.05f;

        foreach (var player in GameController.activePlayers)
        {
            if (!GameController.isTeamMode || (player.team == Team.Red && redScoreDisplay == null) || (player.team == Team.Blue && blueScoreDisplay == null))
            {
                var psd = Instantiate(scoreDisplayPrefab, scoreCanvas.transform) as PlayerScoreDisplay;
                psd.player = player;
                psd.color = player.color;
                psd.text.color = player.color;
                playerScoreDisplays.Add(psd);
                psd.color = player.color;
                psd.text.text = player.roundWins.ToString();
                psd.useRoundWins = true;
                if (GameController.isTeamMode)
                {
                    if (player.team == Team.Red)
                    {
                        redScoreDisplay = psd;
                    }
                    else
                    {
                        blueScoreDisplay = psd;
                    }
                }
            }
        }

        SortScoreboard();
        ArrangeScoreboard(true);
    }


    void SortScoreboard()
    {
        playerScoreDisplays.Sort((x, y) => (y.player.roundWins * 100 + y.player.sortPriority) - (x.player.roundWins * 100 + x.player.sortPriority));
    }


    void ArrangeScoreboard(bool instant)
    {
        for (int i = 0; i < playerScoreDisplays.Count; i++)
        {
            var p = playerScoreDisplays[i].transform.localPosition;
            if (instant)
                p.y = Mathf.Lerp(p.y, i * -2f, 1f);
            else
                p.y = Mathf.Lerp(p.y, i * -2f, Time.deltaTime * 3f);
            p.z = 0f;
            playerScoreDisplays[i].transform.localPosition = p;
        }
    }
    bool haveUpdatedScores;



    // Update is called once per frame
    void Update()
    {
        if (updateScoresDelay > 0f)
        {
            updateScoresDelay -= Time.deltaTime;
            return;
        }
        if (!haveUpdatedScores)
        {
            haveUpdatedScores = true;
            if (GameController.isTeamMode)
            {
                foreach (var player in GameController.activePlayers)
                {
                    if (player.team == GameController.lastWinningPlayer.team)
                    {
                        player.roundWins++;
                    }
                }
            }
            foreach (var psd in playerScoreDisplays)
            {
                if (GameController.isTeamMode)
                {
                    if (psd.player.team == GameController.lastWinningPlayer.team)
                    {
                        if (GameController.isShowDown)
                            psd.TemorarilyDisplay("WINNER!", 10f);
                        else
                            psd.TemorarilyDisplay(psd.player.roundWins.ToString());
                    }
                }
                else
                {
                    if (psd.player == GameController.lastWinningPlayer)
                    {
                        psd.player.roundWins++;
                        if (GameController.isShowDown)
                            psd.TemorarilyDisplay("WINNER!", 10f);
                        else
                            psd.TemorarilyDisplay(psd.player.roundWins.ToString());
                    }
                }
            }

            SortScoreboard();

            if (isLastRound && GameController.AreAnyPlayersTiedForVictory())
            {
                roundText.text = "SHOWDOWN ! ! ! ";
                GameController.isShowDown = true;
            }


        }
        ArrangeScoreboard(false);
        if (isLastRound && GameController.AreAnyPlayersTiedForVictory())
        {
            roundText.color = Time.time % 0.2 < 0.1f ? Color.white : Color.black;
        }

        continueDelay -= Time.deltaTime;
        if (continueDelay <= 0f)
        {
            if (isLastRound && GameController.AreAnyPlayersTiedForVictory())
            {
                GameController.SetupPlayersForShowdown();
                UnityEngine.SceneManagement.SceneManager.LoadScene("7Showdown");
            }
            else if (isLastRound)
            {
                GameController.overallWinnerColor = playerScoreDisplays[0].color;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Outro");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(GameController.levelNames[GameController.levelNo]);
            }
        }

    }
}
