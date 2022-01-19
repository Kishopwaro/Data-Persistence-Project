using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(1000)] //le script s'exécute après tout le reste vu que c'est de l'UI
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject inputField;
    [SerializeField] private TextMeshProUGUI highscoreText;

    private void Start()
    {
        DisplayHighscore();    
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        DataBetweenScenes.Instance.SaveHighscore();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
Application.Quit();
#endif 
    }

    public void UpdatePlayerName()
    {
        DataBetweenScenes.Instance.nameText = inputField.GetComponent<InputField>().text;
    }

    public void DisplayHighscore()
    {
        if(DataBetweenScenes.Instance.highScore > 0)
        {
            highscoreText.SetText(DataBetweenScenes.Instance.highScoreNameText + ": " + DataBetweenScenes.Instance.highScore);
        }
        else
        {
            highscoreText.SetText("-");
        }
        
    }

    public void ResetSaveData()
    {
        DataBetweenScenes.Instance.highScore = 0;
        DataBetweenScenes.Instance.highScoreNameText = "Player";
        DataBetweenScenes.Instance.SaveHighscore();
        DisplayHighscore();
    }
}
