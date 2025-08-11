using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TutorialSwitcher : MonoBehaviour
{
    [Header("Objetos de conteúdo para TECLADO/MOUSE")]
    public GameObject[] keyboardContents;

    [Header("Objetos de conteúdo para CONTROLE")]
    public GameObject[] controllerContents;

    private IDisposable anyButtonListener;

    private void OnEnable()
    {
        // Inscreve o callback e guarda o IDisposable para poder cancelar depois
        anyButtonListener = InputSystem.onAnyButtonPress.Call(OnAnyInput);
    }

    private void OnDisable()
    {
        // Cancela a inscrição de forma segura
        anyButtonListener?.Dispose();
        anyButtonListener = null;
    }

    private void OnAnyInput(InputControl control)
    {
        if (control.device is Keyboard || control.device is Mouse)
            MostrarTeclado();
        else if (control.device is Gamepad)
            MostrarControle();
    }

    private void MostrarTeclado()
    {
        foreach (var obj in keyboardContents)
            obj.SetActive(true);

        foreach (var obj in controllerContents)
            obj.SetActive(false);
    }

    private void MostrarControle()
    {
        foreach (var obj in keyboardContents)
            obj.SetActive(false);

        foreach (var obj in controllerContents)
            obj.SetActive(true);
    }
}
