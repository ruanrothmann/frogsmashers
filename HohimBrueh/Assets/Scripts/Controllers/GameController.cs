using System.Collections;
using System.Collections.Generic;
using FreeLives;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    RoundFinished,
    JoinScreen
}


public class GameController : MonoBehaviour
{
    public static bool charactersBounceEachOther = false;
    public static bool weirdBounceTrajectories = false;
    public static bool onlyBounceBeforeRecover = true;
    public static bool allowTeamMode = false;

    public static List<Player> activePlayers = new List<Player>();

    static List<Player> inactivePlayers;

    GameState state;

    public static Color overallWinnerColor = Color.red;

    public static GameState State { get { return instance.state; } }

    public Character characterPrefab;

    public Fly flyPrefab, activeFly;

    float flySpawnDelay;

    public SpriteRenderer offscreenDotPrefab;

    public Canvas scoreCanvas;

    public List<PlayerScoreDisplay> playerScoreDisplays;

    public PlayerScoreDisplay scoreDisplayPrefab;

    static GameController instance;

    public Color[] playerColors;

    List<Color> availableColors;

    public bool isJoinScreen;

    public static string[] levelNames = new string[] { "1BusStop", "2DownSmash", "3Moon", "4FinalFrogstination", "5Skyline", "6Finale" };
    //public static string[] levelNames = new string[] {  "2DownSmash" };
    public JoinCanvas[] joinCanvas;

    float finishDelay = 7.5f;

    public Text joinCountdownText, joinGameModeText;

    public static bool isTeamMode;

    public static bool playersCanDropIn;

    public static bool isShowDown;

    int redTeamScore, blueTeamScore;
    PlayerScoreDisplay redTeamScoreDisplay, blueTeamScoreDisplay;

    void Awake()
    {
        //Camera.main.aspect = 16f / 9f;
        if (isJoinScreen)
        {
            isShowDown = false;
            state = GameState.JoinScreen;
            finishDelay = 5f;
            SetupForJoinScreen();

            inactivePlayers = null;
            activePlayers.Clear();
            levelNo = 0;
            playersCanDropIn = true;

        }
        playerScoreDisplays = new List<PlayerScoreDisplay>();
        instance = this;
        if (inactivePlayers == null)
        {
            inactivePlayers = new List<Player>();
            Player p;
            p = new Player(FreeLives.InputReader.Device.Gamepad1, playerColors[0], 0);
            inactivePlayers.Add(p);
            p = new Player(FreeLives.InputReader.Device.Gamepad2, playerColors[1], 1);
            inactivePlayers.Add(p);
            p = new Player(FreeLives.InputReader.Device.Gamepad3, playerColors[2], 2);
            inactivePlayers.Add(p);
            p = new Player(FreeLives.InputReader.Device.Gamepad4, playerColors[3], 3);
            inactivePlayers.Add(p);
            p = new Player(FreeLives.InputReader.Device.Keyboard1, playerColors[4], 4);
            inactivePlayers.Add(p);
            p = new Player(FreeLives.InputReader.Device.Keyboard2, playerColors[5], 5);
            inactivePlayers.Add(p);

        }
        else
        {
            int i = 0;
            if (isTeamMode)
            {
                foreach (var player in activePlayers)
                {
                    player.score = 0;
                    player.spawnDelay = 0.5f + 0.2f * i;
                    i++;
                }


                var psd = Instantiate(scoreDisplayPrefab, scoreCanvas.transform) as PlayerScoreDisplay;
                psd.color = Color.red;
                psd.text.color = Color.red;
                redTeamScoreDisplay = psd;
                foreach (var p in activePlayers)
                    if (p.team == Team.Red)
                        psd.player = p;

                playerScoreDisplays.Add(psd);

                psd = Instantiate(scoreDisplayPrefab, scoreCanvas.transform) as PlayerScoreDisplay;
                psd.color = Color.blue;
                psd.text.color = Color.blue;
                blueTeamScoreDisplay = psd;
                foreach (var p in activePlayers)
                    if (p.team == Team.Blue)
                        psd.player = p;
                playerScoreDisplays.Add(psd);

            }
            else
            {
                foreach (var player in activePlayers)
                {
                    player.score = 0;
                    player.spawnDelay = 0.5f + 0.2f * i;
                    i++;
                    var psd = Instantiate(scoreDisplayPrefab, scoreCanvas.transform) as PlayerScoreDisplay;
                    psd.player = player;
                    psd.color = player.color;
                    psd.text.color = player.color;
                    playerScoreDisplays.Add(psd);

                }
            }
        }

        if (isShowDown)
            foreach (var psd in playerScoreDisplays)
            {
                psd.gameObject.SetActive(false);
            }

        InputReader.GetInput(combinedInput);
    }

    internal static void SetupPlayersForShowdown()
    {
        List<Player> winningPlayers = GetLeadingPlayers();
        activePlayers.Clear();
        foreach (var p in winningPlayers)
            activePlayers.Add(p);
        playersCanDropIn = false;
       
    }

    void Start()
    {
        float aspect = ((float)Screen.width / Screen.height);
        float screenWidth = 18f * aspect;
        float adust = 32f / screenWidth;
        Camera.main.orthographicSize = adust * 18f;
    }

    FreeLives.InputState input = new InputState();
    FreeLives.InputState combinedInput = new InputState();
    void Update()
    {

        

        if (state == GameState.JoinScreen)
        {

            for (int i = inactivePlayers.Count - 1; i >= 0; i--)
            {
                InputReader.GetInput(inactivePlayers[i].inputDevice, input);

                if (input.xButton)
                {
                    for (int j = 0; j < joinCanvas.Length; j++)
                    {
                        if (!joinCanvas[j].HasAssignedPlayer())
                        {
                            joinCanvas[j].AssignPlayer(inactivePlayers[i]);
                            inactivePlayers.RemoveAt(i);
                            j = joinCanvas.Length;
                        }
                    }
                }
            }


            int assignedPlayers = 0;

            for (int i = 0; i < joinCanvas.Length; i++)
            {
                if (joinCanvas[i].HasAssignedPlayer())
                {
                    assignedPlayers++;
                }
            }

            bool playersAreReady = CheckReadyPlayers();

            if (assignedPlayers == 0)
            {
                InputReader.GetInput(combinedInput);
                if (combinedInput.start && !combinedInput.wasStart && allowTeamMode)
                {
                    isTeamMode = !isTeamMode;
                    joinGameModeText.text = isTeamMode ? "TEAM" : "FREE  FOR  ALL";
                }

            }
            else
            if (playersAreReady)
            {
                finishDelay -= Time.deltaTime;
                joinCountdownText.text = ((int)(finishDelay) + 1).ToString();
                if (finishDelay <= 0f)
                    FinishRound();
            }
            else
            {
                joinCountdownText.text = "";
                finishDelay = 5f;
            }


        }
        else if (state == GameState.Playing)
        {
            if (activeFly == null && !isShowDown)
            {
                if (flySpawnDelay > 0f)
                {
                    flySpawnDelay -= Time.deltaTime;
                    if (flySpawnDelay <= 0f)
                        activeFly = Instantiate(flyPrefab, Terrain.GetFlySpawnPoint(), Quaternion.identity);
                }
                else
                {
                    flySpawnDelay = Random.Range(15f, 45f);
                }


            }


            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (activePlayers[i].character == null)
                {
                    activePlayers[i].spawnDelay -= Time.deltaTime;
                    if (activePlayers[i].spawnDelay < 0f)
                    {
                        SpawnCharacter(activePlayers[i]);
                    }
                }

                if (activePlayers[i].character != null && activePlayers[i].character.transform.position.y > Terrain.ScreenTop)
                {


                    var spr = activePlayers[i].offscreenDot;
                    if (spr == null)
                    {
                        activePlayers[i].offscreenDot = spr = Instantiate(offscreenDotPrefab);
                        spr.color = activePlayers[i].color;
                    }
                    spr.enabled = true;
                    spr.transform.position = new Vector3(activePlayers[i].character.transform.position.x, Terrain.ScreenTop, -6f);


                }
                else
                {
                    var spr = activePlayers[i].offscreenDot;
                    if (spr != null)
                    {
                        spr.enabled = false;
                    }
                }




            }

            ArrangeScoreboards();

            if (!isShowDown)
            {
                for (int i = inactivePlayers.Count - 1; i >= 0; i--)
                {
                    InputReader.GetInput(inactivePlayers[i].inputDevice, input);

                    if (input.xButton)
                    {
                        print(inactivePlayers[i].color + ", " + inactivePlayers[i].inputDevice);
                        inactivePlayers[i].color = playerColors[Random.Range(0, playerColors.Length)];
                        AddPlayer(inactivePlayers[i]);


                        inactivePlayers.RemoveAt(i);
                    }
                }

                if (Input.GetKeyDown(KeyCode.F2))
                {
                    SpawnCharacter(null);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
            }
        }
        else if (state == GameState.RoundFinished)
        {
            ArrangeScoreboards();

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (activePlayers[i].character == null && winningPlayer == activePlayers[i])
                {
                    activePlayers[i].spawnDelay -= Time.deltaTime;
                    if (activePlayers[i].spawnDelay < 0f)
                    {
                        SpawnCharacter(activePlayers[i]);
                    }
                }
            }


            finishDelay -= Time.deltaTime;
            if (finishDelay < 0f)
                FinishRound();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            showGui = !showGui;
        }
    }

    void SetupForJoinScreen()
    {
        availableColors = new List<Color>();
        availableColors.AddRange(playerColors);

    }

    bool CheckReadyPlayers()
    {
        int readyPlayers = 0;
        if (GameController.isTeamMode)
        {
            bool redTeamHasPlayer = false, blueTeamHasPlayer = false;
            for (int i = 0; i < joinCanvas.Length; i++)
            {
                if (joinCanvas[i].HasAssignedPlayer())
                {
                    if (joinCanvas[i].state == JoinCanvas.State.Ready)
                    {
                        readyPlayers++;
                        if (joinCanvas[i].assignedPlayer.team == Team.Blue)
                            blueTeamHasPlayer = true;
                        else if (joinCanvas[i].assignedPlayer.team == Team.Red)
                            redTeamHasPlayer = true;
                    }
                    else
                    {
                        readyPlayers = -100;
                    }
                }
            }

            return redTeamHasPlayer && blueTeamHasPlayer && readyPlayers >= 2;
        }
        else
        {
            for (int i = 0; i < joinCanvas.Length; i++)
            {
                if (joinCanvas[i].HasAssignedPlayer())
                {
                    if (joinCanvas[i].state == JoinCanvas.State.Ready)
                        readyPlayers++;
                    else
                    {
                        readyPlayers = -100;
                    }
                }
            }
            return readyPlayers >= 2;
        }
    }

    void ArrangeScoreboards()
    {
        for (int i = 0; i < playerScoreDisplays.Count; i++)
        {
            var p = playerScoreDisplays[i].transform.localPosition;
            //print(p);
            p.y = Mathf.Lerp(p.y, i * -2f, Time.deltaTime * 3f);
            p.z = 0f;
            //print(p);
            playerScoreDisplays[i].transform.localPosition = p;


        }
    }

    public static int levelNo;
    void FinishRound()
    {
        if (state == GameState.JoinScreen)
        {
            levelNo = 0;
            foreach (var jc in joinCanvas)
            {
                if (jc.HasAssignedPlayer())
                    activePlayers.Add(jc.assignedPlayer);

            }
            SceneManager.LoadScene(levelNames[0]);
        }
        else
        {
            levelNo++;
            //if (levelNo >= 5)
            {
                SceneManager.LoadScene("ScoreScreen");
            }
        }
    }

    void AddPlayer(Player player)
    {
        activePlayers.Add(player);

        var psd = Instantiate(scoreDisplayPrefab, scoreCanvas.transform) as PlayerScoreDisplay;
        psd.player = player;
        psd.color = player.color;
        psd.text.color = player.color;
        playerScoreDisplays.Add(psd);

    }


    void SpawnCharacter(Player player)
    {
        var point = Terrain.GetSpawnPoint();
        var ch = Instantiate(characterPrefab, point, Quaternion.identity) as Character;
        if (player != null)
        {
            ch.player = player;
            player.character = ch;
            player.spawnDelay = 1f;

            EffectsController.CreateSpawnEffects(point + Vector3.up, player.color);
            SoundController.PlaySoundEffect("CharacterSpawn", 0.4f, point);
        }
    }

    public static void SpawnCharacterJoinScreen(Player player)
    {
        for (int i = 0; i < instance.joinCanvas.Length; i++)
        {
            if (instance.joinCanvas[i].assignedPlayer == player)
            {
                var ch = Instantiate(instance.characterPrefab, Terrain.GetSpawnPoint(i), Quaternion.identity) as Character;
                ch.player = player;
                player.character = ch;
            }
        }
    }

    Player winningPlayer;

    public static Player lastWinningPlayer { get; private set; }
    public static bool HasInstance { get { return instance != null; } }

    public static Player GetWinningPlayer()
    {
        if (State == GameState.RoundFinished)
        {
            return instance.winningPlayer;
        }
        return null;
    }

    void SortScoreboard()
    {
        if (GameController.isTeamMode)
        {
            redTeamScoreDisplay.player.score = redTeamScore;
            blueTeamScoreDisplay.player.score = blueTeamScore;
        }
        instance.playerScoreDisplays.Sort((x, y) => (y.player.score * 100 + y.player.sortPriority) - (x.player.score * 100 + x.player.sortPriority));
    }


    internal static void RegisterKill(Player gotPoint, Player gotKilled, int hits)
    {



        if (State == GameState.RoundFinished)
            return;

        if (isShowDown)
        {
            bool wonRound = false;
            if (activePlayers.Contains(gotKilled))
            {
                activePlayers.Remove(gotKilled);
                if (gotKilled.offscreenDot != null)
                    GameObject.Destroy(gotKilled.offscreenDot);
            }
            if (activePlayers.Count == 1)
            {
                wonRound = true;
                var winner = activePlayers[0];
                if (winner.character != null)
                    winner.character.GetComponent<ScorePlum>().ShowText("WIN!", 5f);
                instance.winningPlayer = winner;
                lastWinningPlayer = winner;
            }
            else if (isTeamMode)
            {
                bool winnersContainRed = false;
                bool winnersContainBlue = false;
                foreach (var player in activePlayers)
                {
                    if (player.team == Team.Red)
                        winnersContainRed = true;
                    else winnersContainBlue = true;
                }

                if (!winnersContainBlue || !winnersContainRed)
                {
                    wonRound = true;
                    var winner = activePlayers[0];
                    if (winner.character != null)
                        winner.character.GetComponent<ScorePlum>().ShowText("WIN!", 5f);
                    instance.winningPlayer = winner;
                    lastWinningPlayer = winner;
                }

            }

            if (wonRound)
            {
                SoundController.PlaySoundEffect("VictorySting", 0.5f);
                SoundController.StopMusic();
                instance.state = GameState.RoundFinished;
            }
            return;
        }

        if (gotPoint != null)
        {
            if (hits <= 0)
                hits = 1;

            if (GameController.isTeamMode)
            {
                if (gotPoint.team == Team.Blue)
                    instance.blueTeamScore += hits;
                else
                    instance.redTeamScore += hits;
            }
            else
            {
                gotPoint.score += hits;
            }

            bool wonRound = false;
            if (GameController.isTeamMode)
            {
                wonRound = ((gotPoint.team == Team.Red && instance.redTeamScore >= 10) || (gotPoint.team == Team.Blue && instance.blueTeamScore >= 10));
            }
            else
            {
                if (activePlayers.Count == 2)
                    wonRound = gotPoint.score >= 5;
                else
                    wonRound = gotPoint.score >= 10;
            }
            if (wonRound)
            {
                SoundController.PlaySoundEffect("VictorySting", 0.5f);
                SoundController.StopMusic();
                instance.state = GameState.RoundFinished;
                instance.GetPlayerScoreDisplay(gotPoint).TemorarilyDisplay("WINNER ! ! !", 5f);
                if (gotPoint.character != null)
                    gotPoint.character.GetComponent<ScorePlum>().ShowText("WIN!", 5f);
                instance.winningPlayer = gotPoint;
                lastWinningPlayer = gotPoint;
            }
            else
            {
                instance.GetPlayerScoreDisplay(gotPoint).TemorarilyDisplay("+" + hits.ToString());
                if (gotPoint.character != null)
                    gotPoint.character.GetComponent<ScorePlum>().ShowText("+" + hits.ToString());
            }




        }
        else if (gotKilled != null)
        {

            //if (isTeamMode)
            //{
            //    if (gotKilled.team == Team.Blue)
            //        instance.blueTeamScore--;
            //    else
            //        instance.redTeamScore--;
            //}
            //else
            //{
            //    gotKilled.score--;
            //}
            //instance.GetPlayerScoreDisplay(gotKilled).TemorarilyDisplay("-" + 1);
        }

        instance.SortScoreboard();


    }

    PlayerScoreDisplay GetPlayerScoreDisplay(Player player)
    {
        if (isTeamMode)
        {
            if (player.team == Team.Red)
                return redTeamScoreDisplay;
            else
                return blueTeamScoreDisplay;
        }
        else
        {
            for (int i = 0; i < playerScoreDisplays.Count; i++)
            {
                if (playerScoreDisplays[i].player == player)
                    return playerScoreDisplays[i];
            }
        }
        return null;
    }

    bool showGui;

    public void OnGUI()
    {
        if (showGui)
        {
            GUILayout.BeginArea(new Rect(0, 0, 400, 400));
            charactersBounceEachOther = GUILayout.Toggle(charactersBounceEachOther, "Characters Bounce Each Other");
            weirdBounceTrajectories = GUILayout.Toggle(weirdBounceTrajectories, "Weird Bounce Trajectories");
            onlyBounceBeforeRecover = GUILayout.Toggle(onlyBounceBeforeRecover, "Only Bounce Before Recover");
            GUILayout.EndArea();
        }
    }

    public static Color GetAvailableColor()
    {
        int i = UnityEngine.Random.Range(0, instance.availableColors.Count);
        var col = instance.availableColors[i];
        instance.availableColors.RemoveAt(i);
        return col;
    }

    public static void ReturnColor(Color color)
    {
        instance.availableColors.Add(color);
    }

    public static void ReturnPlayer(Player player)
    {
        activePlayers.Remove(player);
        inactivePlayers.Add(player);
    }


    public static List<Player> GetLeadingPlayers()
    {
        int topScore = -1;
        List<Player> tiedPlayers = new List<Player>();
        foreach (var player in activePlayers)
        {
            if (player.roundWins == topScore)
            {
                tiedPlayers.Add(player);
            }
            else if (player.roundWins > topScore)
            {
                topScore = player.roundWins;
                tiedPlayers.Clear();
                tiedPlayers.Add(player);
            }
        }

        return tiedPlayers;
    }

    public static bool AreAnyPlayersTiedForVictory()
    {
        int topScore = -1;
        List<Player> tiedPlayers = new List<Player>();
        foreach (var player in activePlayers)
        {
            if (player.roundWins == topScore)
            {
                tiedPlayers.Add(player);
            }
            else if (player.roundWins > topScore)
            {
                topScore = player.roundWins;
                tiedPlayers.Clear();
                tiedPlayers.Add(player);
            }
        }
        if (GameController.isTeamMode)
        {
            bool redIsTied = false, blueIsTied = false;
            foreach (var p in tiedPlayers)
            {
                if (p.team == Team.Red)
                    redIsTied = true;
                if (p.team == Team.Blue)
                    blueIsTied = true;
            }
            return redIsTied && blueIsTied;
        }
        else
            return tiedPlayers.Count > 1;

    }

}
