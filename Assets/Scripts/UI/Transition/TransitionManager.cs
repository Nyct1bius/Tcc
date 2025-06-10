using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    public static TransitionManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<TransitionManager>();
            return instance;
        }
    }

    public TransitionLayer layer { get; private set; }

    private void Awake()
    {
        layer = GetComponentInChildren<TransitionLayer>();
    }

    private void Start()
    {
        if (!layer.isDone)
        {
            layer.HideImmediately();
        }
    }

}
