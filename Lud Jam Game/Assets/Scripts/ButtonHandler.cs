using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {
    public GameObject TutorialPanel;

    public GameObject CreditsPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }

    public void ShowTutorial() {
        TutorialPanel.SetActive(!TutorialPanel.activeSelf);
    }

    public void ShowCredits() {
        CreditsPanel.SetActive(!CreditsPanel.activeSelf);
    }

    public void Quit() {
        Application.Quit();
    }
}
