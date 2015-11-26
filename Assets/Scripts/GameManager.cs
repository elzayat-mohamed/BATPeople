using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

    public PlayerController[] playerControllers;

    public ObstacleViewGenerator obstacleViewGenerator;
    public FreeParallax paralex;

    public CameraController cameraController;
    public GuiController guiController;
    public AudioController audioController;

    private float timeSinceRunStarted;

    private GameStates currentGameState;
	public InputAction inputActions;

    void Awake()
    {
        if (obstacleViewGenerator == null) throw new MissingReferenceException("Missing a reference to the ObstacleViewGenerator on " + this.gameObject);
        if (paralex == null) throw new MissingReferenceException("Missing a reference to the FreeParallax on " + this.gameObject);
        if (cameraController == null) throw new MissingReferenceException("Missing a reference to the CameraController on the " + this.gameObject);

        currentSelectedPlayerIndex = -1;
        paralex.Play();

        //GUI
        guiController.OnMenuButtonClick += () => Application.LoadLevel(Application.loadedLevel); 
        guiController.OnMenuButtonClick += audioController.Menu;
       
        guiController.OnPauseButtonClick += audioController.Menu;
        guiController.OnPauseButtonClick += Pause;
        
        guiController.OnRetryButtonClick += Retry;
        guiController.OnRetryButtonClick += audioController.InGame;
        guiController.OnBackButtonClick += audioController.InGame;
        guiController.OnBackButtonClick += Pause;

        guiController.OnQuitButtonClick += Application.Quit;
        guiController.OnSoundfxToggle += audioController.SfxToggle;
        guiController.OnMusicToggle += audioController.MusicToggle;

		inputActions.OnTap += HandleOnTap;
		inputActions.OnSwipe += HandleOnSwipe;

        currentGameState = GameStates.Menu;
    }

    void HandleOnSwipe ()
    {
		if (currentGameState == GameStates.Menu) 
		{
			NextPlayer ();
		}
    }

    void HandleOnTap ()
    {

		if (currentGameState == GameStates.Menu) 
		{
			PlayGame();
		}
    }

    private void SelectCharacter()
    {
        obstacleViewGenerator.Initialize(selectedPlayer.transform);
        cameraController.Follow(selectedPlayer.transform);

        //NextPlayer();

        selectedPlayer.OnGameOver += cameraController.GameOver;
      
        selectedPlayer.OnGameOver += guiController.GameOver;
        selectedPlayer.OnGameOver += audioController.GameOver;
        selectedPlayer.OnGameOver += GameOver;
        
        selectedPlayer.OnRun += cameraController.ZoomOut;
        selectedPlayer.OnRun += audioController.InGame;
        
        selectedPlayer.OnJump += audioController.Jump;


        obstacleViewGenerator.Generate();
        currentHighScore = GetHighScore();
    }


    void Start()
    {
        Helper.Instance.Delay(0.1f, NextPlayer);
    }

    private int currentHighScore;
    void Update()
    {
        if (currentGameState == GameStates.InGame)
        {
            timeSinceRunStarted += Time.deltaTime;
            selectedPlayer.CalculateSpeed(timeSinceRunStarted);
            obstacleViewGenerator.CalculateSpeed(timeSinceRunStarted);
            guiController.UpdateScores(selectedPlayer.score, currentHighScore);

        }
        else if (currentGameState == GameStates.Menu)
        { 
            //Swipe
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NextPlayer();
            }

//            var inputAction = InputAction.GetInput();
//
//            if (inputAction == InputType.Down)
//            {
//                NextPlayer();
//            }
//
//            if (Input.GetKeyDown(KeyCode.Space) || inputAction == InputType.Tap)
//            {
//                PlayGame();
//            }
        }

    }

    void PlayGame()
    {
        Time.timeScale = 1;
        SelectCharacter();

        currentGameState = GameStates.InGame;
        
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (i != currentSelectedPlayerIndex)
            {
                playerControllers[i].Disable();
            }
        }

        selectedPlayer.Run();
        timeSinceRunStarted = 0;


        ShowLastSessionBillboard();
    }
    void PlayMenu()
    {
        Time.timeScale = 1;
        currentGameState = GameStates.Menu;

        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i].Enable();
        }
    }
    void Retry()
    {
        timeSinceRunStarted = 0;

        selectedPlayer.Restart();
        cameraController.Restart();

        obstacleViewGenerator.Generate();

        currentGameState = GameStates.InGame;
        paralex.Play();

        ShowLastSessionBillboard();
    }

    void Pause()
    {
        if (currentGameState == GameStates.Pause)
        {
            Time.timeScale = 1;
            currentGameState = GameStates.InGame;
        }
        else
        {
            Time.timeScale = 0;
            currentGameState = GameStates.Pause;
        }
    }

    void GameOver()
    {
        SaveLastPlayerLevelLocationX(selectedPlayer.FailLocation.x);

        paralex.Stop();

        SaveHighScore(selectedPlayer.score);
        currentHighScore = GetHighScore();
        guiController.UpdateScores(selectedPlayer.score, currentHighScore);
        
        timeSinceRunStarted = 0;



        ChangeStart();
       
    }
    private int currentSelectedPlayerIndex;
    public void NextPlayer()
    {
        if (currentSelectedPlayerIndex != -1)
        {
            playerControllers[currentSelectedPlayerIndex].JumpFromBuilding();
        }

        currentSelectedPlayerIndex++;
        if (currentSelectedPlayerIndex >= playerControllers.Length)
        {
            currentSelectedPlayerIndex = 0;
        }
		playerControllers[currentSelectedPlayerIndex].JumpOnBuilding();

    }

    private PlayerController selectedPlayer { get { return playerControllers[currentSelectedPlayerIndex]; } }

    public GameObject BuildingObject;
    public GameObject Clif;

    private void ChangeStart()
    {
        BuildingObject.SetActive(!BuildingObject.activeSelf);
        Clif.SetActive(!BuildingObject.activeSelf);
    }

    //Billboard
    public GameObject billboardObject;
    private void ShowLastSessionBillboard()
    {
        

        var lastLevelLocationX = GetLasPlayerLevelFailLocationX();
        //if (lastLevelLocationX > 50)
        {
            billboardObject.transform.position = new Vector3(lastLevelLocationX, 0, 15f);
            billboardObject.SetActive(true);
        }
    }

    //Persistence
    private string lastLevelLocationXKey = "lastLevelLocationX ";
    private string highScoreKey = "highScoreKey";

    public void SaveLastPlayerLevelLocationX(float location)
    {
        PlayerPrefs.SetFloat(lastLevelLocationXKey, location);
    }
    public float GetLasPlayerLevelFailLocationX()
    {
        return PlayerPrefs.GetFloat(lastLevelLocationXKey, 0);        
    }
    
    public void SaveHighScore(int score)
    {
        if (PlayerPrefs.GetInt(highScoreKey, 0) < score)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(highScoreKey, 0);
    }
}

public enum GameStates
{
    Null,
    InGame,
    GameOver,
    Pause,
    Menu
}