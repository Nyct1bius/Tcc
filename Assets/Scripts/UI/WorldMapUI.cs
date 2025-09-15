using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [Header("Refer�ncias")]
    public RectTransform mapaImage;       // A imagem do mapa (RectTransform da UI)
    public RectTransform playerIcon;      // O �cone do jogador (UI)
    public GameObject mapaPanel;          // O painel do mapa (UI)
    public Transform player;              // O Transform do jogador no mundo

    [Header("Limites do mundo (X/Z)")]
    public float minX = -700f;
    public float maxX = 276f;
    public float minZ = -377f;
    public float maxZ = 172f;

    void Update()
    {
        // Alterna o mapa com a tecla P
        if (Input.GetKeyDown(KeyCode.M))
        {
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
}
