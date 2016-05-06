using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

	// Use this for initialization


	public Text scoreTextMesh, finalScoreTextMesh, BestScoreTextMesh;
	int inGameScore = 0;
	public GameObject newScoreObj, ScoreBoard, MainMenuContainer, Tutorial, QuitButton, SubmitScore, hudmenu, Leaderboards, LeaderboardSubmit, EnterNameObj, Setting, LeaderboardEnding;
    public Text[] highscoreFields;
    Highscores highscoresManager;
    DisplayHighscores highscoreDisplay;
    public InputField enterName;


    public static bool isRestartPressed = false;
	bool readyToPlay = false;

	void OnEnable ()
	{
		
		playerController.IncreaseScore += OnScoreIncrease;
		playerController.PlayerDead += OnPlayerDead;

	
		MainMenuContainer.SetActive (true);
		newScoreObj.SetActive (false);
		ScoreBoard.SetActive (false);
		hudmenu.SetActive (false);
        Leaderboards.SetActive(false);
        LeaderboardSubmit.SetActive(false);

		//on game end scoreboard menu ,if user presses on restart ,
		//we need to disable the mainmenu and changing player to readyto play.
		if (isRestartPressed) {
			MainMenuContainer.SetActive (false);
			isRestartPressed = false;
			readyToPlay = true;
			hudmenu.SetActive (true);
			playerController.currentState = playerController.playerStates.alive;

		} else {
			playerController.currentState = playerController.playerStates.idle;

		}

		scoreTextMesh.text = "" + 0;

	}
	 

    void Update ()
	{

 
	
		//to handle escape key
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (playerController.currentState == playerController.playerStates.idle) {
				
				Application.Quit ();
			} else {
				
				restart ();
			}
			
		}
	}

	public void OnButtonClicks (string incomingName)
	{
	//	Debug.Log ("clicked on " + incomingName);
		SoundController.Static.PlayClickSound ();

		switch (incomingName) {
		case "Play":
			MainMenuContainer.SetActive (false);
			Tutorial.SetActive (true);
			readyToPlay = true;
			playerController.currentState = playerController.playerStates.alive;

			hudmenu.SetActive (true);
			Invoke ("lateDeactivateTutorial", 3);
			break;

		case "Home":
			
			restart ();
			break;
			
		case "Restart":
			isRestartPressed = true;
		
			hudmenu.SetActive (true);
			restart ();

			break;

            case "Setting":
                Setting.SetActive(true);
                break;

            case "BackSetting":
                Setting.SetActive(false);
                break;

        case "Leaderboards":
                
            Leaderboards.SetActive(true);
            break;

        case "Back":
            Leaderboards.SetActive(false);

            break;

        case "LeaderboardSubmit":
            LeaderboardSubmit.SetActive(true);

            break;

        case "BackSubmit":
            LeaderboardSubmit.SetActive(false);
            ScoreBoard.SetActive(true);

            break;

            case "LeaderboardEnding":
                LeaderboardEnding.SetActive(true);
                break;

            case "BackLeaderboardEnding":
                LeaderboardEnding.SetActive(false);
                break;
			
		case "Quit":
			
			Application.Quit();
			
			break;
		}
	}

    public void lateDeactivateTutorial ()
	{
		Tutorial.SetActive (false);
	}

	public void restart ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		//Application.LoadLevel (Application.loadedLevelName);
	}

	void OnScoreIncrease (System.Object obj, EventArgs args)
	{
		inGameScore++;
		scoreTextMesh.text = "" + inGameScore;
		 
		SoundController.Static.PlayScoreIncrease ();
	}

	void OnPlayerDead (System.Object obj, EventArgs args)
	{
		hudmenu.SetActive (false);
		ScoreBoard.SetActive (true);

      
		finalScoreTextMesh.text = "Score : " + inGameScore;

		if (PlayerPrefs.GetInt ("BestScore", 0) < inGameScore)
        {
			PlayerPrefs.SetInt ("BestScore", inGameScore);
			newScoreObj.SetActive (true);
		}

		BestScoreTextMesh.text = "Best : " + PlayerPrefs.GetInt ("BestScore", inGameScore);

	}

	void OnDisable ()
	{

		playerController.IncreaseScore -= OnScoreIncrease;
		playerController.PlayerDead -= OnPlayerDead;

		 
	}

    void EnterName()
    {
        if (enterName.text != string.Empty)
        {
            int score = inGameScore;
            string username = "";

            username = enterName.text;

            Highscores.AddNewHighscore(username, score);

            EnterNameObj.SetActive(false);
            LeaderboardEnding.SetActive(false);
        }

    }

    IEnumerator RefreshHighscores()
    {
        while (true)
        {
            highscoresManager.DownloadHighscores();
            yield return new WaitForSeconds(30);
        }
    }

}
