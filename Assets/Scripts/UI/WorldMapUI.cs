using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [Header("Refer�ncias")]
    public RectTransform mapaImage;       // A imagem do mapa (RectTransform da UI)
    public RectTransform playerIcon;      // O �cone do jogador (UI)
    public GameObject mapaPanel;          // O painel do mapa (UI)
    public Transform player;              // O Transform do jogador no mundo

    [Header("Refer�ncias do Mundo")]
    public Transform worldBottomLeft; // Ponto inferior esquerdo do mundo
    public Transform worldTopRight;   // Ponto superior direito do mundo

    private float minX, maxX, minZ, maxZ;

    void Start()
    {
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
            Debug.LogWarning("WorldMapUI: Pontos de refer�ncia n�o atribu�dos!");
        }
    }

    void Update()
    {
        // Alterna o mapa com a tecla P
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(player == null)
            {
                GetPlayerReference();
            }

            mapaPanel.SetActive(!mapaPanel.activeSelf);
        }

        // Se o mapa estiver ativo, atualiza a posi��o do jogador
        if (mapaPanel.activeSelf && player != null)
        {
            AtualizarPosicaoJogador(player.position);
        }
    }

    void AtualizarPosicaoJogador(Vector3 posMundo)
    {
        // Normaliza a posi��o do jogador entre 0 e 1
        float xNorm = Mathf.InverseLerp(minX, maxX, posMundo.x);
        float yNorm = Mathf.InverseLerp(minZ, maxZ, posMundo.z);

        // Converte para coordenadas locais da imagem do mapa
        float x = (xNorm * mapaImage.rect.width) - (mapaImage.rect.width / 2f);
        float y = (yNorm * mapaImage.rect.height) - (mapaImage.rect.height / 2f);

        // Atualiza a posi��o do �cone
        playerIcon.localPosition = new Vector3(x, y, 0f);
    }

    void GetPlayerReference()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }

    /*
    Posi��o obj para calcular tamnha do mundo (mapa nao esta centralizado)
    X = -212
    Y = 5.9   (nao importa para o mapa)
    Z = -133

    Escala do obj usado para calcular tamanho do mundo 
    X = 376
    Z = 376

    Calcular metade da escala 
    Half = 376 / 2 = 188

    Limites em X
    MinX = -212 ? 188 = -400
    MaxX = -212 + 188 = -24

    Limites em Z
    MinZ = -133 ? 188 = -321
    MaxZ = -133 + 188 =  55
    */
}
