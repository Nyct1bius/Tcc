using System;
using System.Collections;
using UnityEngine;

public class OceanTest : MonoBehaviour
{
    public static Action OnOceanDown, OnOceanUp;
    [SerializeField] private float timeBetweenStates;
    private enum OceanStates
    {
        down,
        up
    }
    [SerializeField] private OceanStates state;

    private void Start()
    {
        state = OceanStates.up;
        SelectState();
    }


    private void SelectState()
    {
        switch (state)
        {
            case OceanStates.down:
                OnOceanDown?.Invoke();
                break;
            case OceanStates.up:
                OnOceanUp?.Invoke();
                break;
        }
        StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(timeBetweenStates);
        if(state == OceanStates.down)
        {
            state = OceanStates.up;
        }
        else
        {
            state = OceanStates.down;
        }
        SelectState();
    }









}
