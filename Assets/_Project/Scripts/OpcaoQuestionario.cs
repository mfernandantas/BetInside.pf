using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpcaoQuestionario : MonoBehaviour
{
    [Header("Componentes")]
    public Image fundo;
    public GameObject indicadorAtivo;
    public TMP_Text textoPrincipal;
    public TMP_Text textoSubtitulo;

    [Header("Cores")]
    public Color corNormal = new Color32(24, 25, 37, 255);
    public Color corSelecionada = new Color32(50, 12, 28, 255);
    public Color rosa = new Color32(255, 45, 85, 255);
    public Color branco = Color.white;
    public Color lilas = new Color32(160, 165, 192, 255);

    public void DefinirSelecionada(bool selecionada)
    {
        fundo.color = selecionada ? corSelecionada : corNormal;

        indicadorAtivo.SetActive(selecionada);

        textoPrincipal.color = selecionada ? rosa : branco;
        textoSubtitulo.color = lilas;
    }
}