using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Canvas))]
public class GuiController : MonoBehaviour 
{

	public Animator animator;

    [Header("Pause panel")]
	public Button backButtonPause;
	public Button quitButtonPause;
    public Button menuButtonPause;
    public Toggle soundfxToggle;
    public Toggle musicToggle;

    [Header("Play panel")]
	public Button pauseButtonPlay;
    public Text highscoreTextPlay;
    public Text scoreTextPlay;
	
    [Header("Retry panel")]
    public Button menuButtonRetry;
    public Button retryButtonRetry;
	public Text highscoreRetry;
	public Text scoreRetry;


	public event Action OnRetryButtonClick;
	public event Action OnBackButtonClick;
	public event Action OnQuitButtonClick;
	public event Action OnPauseButtonClick;
	public event Action OnMenuButtonClick;

	public event Action<bool> OnSoundfxToggle;
	public event Action<bool> OnMusicToggle;

	void Awake()
	{
        CheckReferences();
		WireUpEvents ();
	}

    private void CheckReferences()
    {
        if (retryButtonRetry == null) throw new MissingReferenceException("GuiController is missing reference to the retryButton button.");
        if (backButtonPause == null) throw new MissingReferenceException("GuiController is missing reference to the backButton button.");
        if (quitButtonPause == null) throw new MissingReferenceException("GuiController is missing reference to the quitButton button.");
        if (pauseButtonPlay == null) throw new MissingReferenceException("GuiController is missing reference to the pauseButton button.");
        if (menuButtonPause == null) throw new MissingReferenceException("GuiController is missing reference to the menuButton button.");
        if (menuButtonRetry == null) throw new MissingReferenceException("GuiController is missing reference to  menuButton2 button.");

        if (animator == null) throw new MissingReferenceException("GuiController is missing reference to the Animator.");

        if (highscoreTextPlay == null) throw new MissingReferenceException("GuiController is missing reference to the highscoreValueText text.");
        if (scoreTextPlay == null) throw new MissingReferenceException("GuiController is missing reference to the scoreValueText text.");
        if (highscoreRetry == null) throw new MissingReferenceException("GuiController is missing reference to the highscoreFinalValueText text.");
        if (scoreRetry == null) throw new MissingReferenceException("GuiController is missing reference to the scoreFinalValueText text.");

        if (soundfxToggle == null) throw new MissingReferenceException("GuiController is missing reference to the soundFxToggle button.");
        if (musicToggle == null) throw new MissingReferenceException("GuiController is missing reference to the musicToggle button.");
    }

	private void WireUpEvents()
	{

        retryButtonRetry.onClick.AddListener(() => { if (OnRetryButtonClick != null) OnRetryButtonClick(); });
        backButtonPause.onClick.AddListener(() => { if (OnBackButtonClick != null) OnBackButtonClick(); });
        quitButtonPause.onClick.AddListener(() => { if (OnQuitButtonClick != null) OnQuitButtonClick(); });
        pauseButtonPlay.onClick.AddListener(() => { if (OnPauseButtonClick != null) OnPauseButtonClick(); });
        menuButtonPause.onClick.AddListener(() => { if (OnMenuButtonClick != null) OnMenuButtonClick(); });
        menuButtonRetry.onClick.AddListener(() => { if (OnMenuButtonClick != null) OnMenuButtonClick(); });

        soundfxToggle.onValueChanged.AddListener((isEnabled) => { if (OnSoundfxToggle != null) OnSoundfxToggle(isEnabled); });
        musicToggle.onValueChanged.AddListener((isEnabled) => { if (OnMusicToggle != null) OnMusicToggle(isEnabled); });


		OnRetryButtonClick += Retry;
		OnBackButtonClick += Back;
		OnPauseButtonClick += OpenMenu;
	}

	public void GameOver()
	{
		animator.SetBool ("Retry", true);
	}

	public void UpdateScores(int currentScore, int highScore)
	{
		highscoreTextPlay.text = highScore.ToString();
		scoreTextPlay.text = currentScore.ToString();
		highscoreRetry.text = highScore.ToString();
		scoreRetry.text = currentScore.ToString();
	}

	void OpenMenu ()
	{
		animator.SetBool ("Pause", true);
	}

	void Back ()
	{
		animator.SetBool ("Pause", false);
	}

	void Retry ()
	{
        animator.SetBool ("Retry", false);
		
		scoreTextPlay.text = "0";
		scoreRetry.text = "0";

	}
}