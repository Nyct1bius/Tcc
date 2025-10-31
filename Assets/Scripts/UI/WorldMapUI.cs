using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [Header("Referências")]
    public RectTransform mapaImage;       // A imagem do mapa (RectTransform da UI)
    public RectTransform playerIcon;      
    public GameObject mapaPanel;          
    public Transform player;              

    [Header("Referências do Mundo")]
    public Transform worldBottomLeft; 
    public Transform worldTopRight;

    
    private PlayerInputs playerInput;

    private float minX, maxX, minZ, maxZ;

    private void Awake()
    {
        playerInput = new PlayerInputs();
    }

    void Start()
    {
        playerInput.Enable();

        // Calcula os limites do mundo com base nos dois pontos
        if (worldBottomLeft != null && worldTopRight != null)
        {
            minX = worldBottomLeft.position.x;
            minZ = worldBottomLeft.position.z;
            maxX = worldTopRight.position.x;
            maxZ = worldTopRight.position.z;
        }
        else
        {
            Debug.LogWarning("WorldMapUI: Pontos de referência não atribuídos!");
        }
    }

    void Update()
    {

        playerInput.UI.Map.performed += OnMapPressed;
        
        if (mapaPanel.activeSelf && player != null)
        {
            AtualizarPosicaoJogador(player.position);
        }
    }

    private void OnMapPressed(InputAction.CallbackContext ctx)
    {
        
        if(player == null)
        {
            GetPlayerReference();
        }

        mapaPanel.SetActive(!mapaPanel.activeSelf);
    }

    void AtualizarPosicaoJogador(Vector3 posMundo)
    {
        // Normaliza a posição do jogador entre 0 e 1
        float xNorm = Mathf.InverseLerp(minX, maxX, posMundo.x);
        float yNorm = Mathf.InverseLerp(minZ, maxZ, posMundo.z);

        // Converte para coordenadas locais da imagem do mapa
        float x = (xNorm * mapaImage.rect.width) - (mapaImage.rect.width / 2f);
        float y = (yNorm * mapaImage.rect.height) - (mapaImage.rect.height / 2f);

        // Atualiza a posição do ícone
        playerIcon.localPosition = new Vector3(x, y, 0f);

        // Pega a rotação do jogador no eixo Y e inverte para alinhar com a UI
        float rotY = -player.eulerAngles.y;
        playerIcon.localRotation = Quaternion.Euler(0f, 0f, rotY);
    }

    void GetPlayerReference()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }
}
