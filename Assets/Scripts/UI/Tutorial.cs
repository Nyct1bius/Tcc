using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Animator tutorialAnimator;

    private bool isFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isFinished)
        {
            if (other.CompareTag("Player")) // Garante que só o jogador ativa o tutorial
            {
                tutorialAnimator.SetTrigger("Show");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!isFinished)
        {
            if (other.CompareTag("Player")) // Garante que só o jogador desativa
            {
                tutorialAnimator.SetTrigger("Hide");
                isFinished = true;
            }
        }
    }
}
