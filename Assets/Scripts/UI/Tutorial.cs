using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TutorialType
{
    Attack,
    Jump,
    Dash,
    Puzzle,
    Navigation
}

public class Tutorial : MonoBehaviour
{
    public TutorialType tutorialType;
    [SerializeField] private GameObject[] tutorialParts;


    private int currentPartIndex = 0;
    private bool isFinished = false;
    private bool isShowing = false;
    PlayerInputs playerInputs;

    private void OnDisable()
    {
        if(playerInputs != null)
        {
            playerInputs.UI.Disable();
            playerInputs.UI.CloseMenu.performed -= CloseTutorial;
        }
        
    }

    private void ShowTutorial()
    {
        if (tutorialParts.Length == 0 || isFinished) return;
        tutorialParts[currentPartIndex].SetActive(true);
        isShowing = true;

        PauseGameManager.PauseGame();
    }

    //private void ShowNextPart()
    //{
    //    // Desativa a parte atual
    //    tutorialParts[currentPartIndex].SetActive(false);

    //    currentPartIndex++;

    //    if(currentPartIndex < tutorialParts.Length)
    //    {
    //        // Ativa a próxima parte
    //        tutorialParts[currentPartIndex].SetActive(true);
    //    }
    //    else
    //    {
    //        //// Se não houver mais partes, fecha o tutorial
    //        //CloseTutorial();
    //    }
    //}

    private void CloseTutorial(InputAction.CallbackContext context)
    {
        tutorialParts[currentPartIndex].SetActive(false);
        if (currentPartIndex + 1 < tutorialParts.Length)
        {
            currentPartIndex++;
            tutorialParts[currentPartIndex].SetActive(true);
            return;
        }
            
        tutorialParts[currentPartIndex].SetActive(false);
        isFinished = true;
        isShowing = false;
        PauseGameManager.ResumeGame();

        playerInputs.UI.Disable();
        playerInputs.UI.CloseMenu.performed -= CloseTutorial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFinished && GameManager.instance.PlayerInstance == other.gameObject)
        {
            ShowTutorial();
            playerInputs = new PlayerInputs();
            playerInputs.UI.Enable();
            playerInputs.UI.CloseMenu.performed += CloseTutorial;

        }
    }
}
