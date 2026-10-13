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

    public void AtualizarTexto(string texto)
    {
        if (textoPrincipal != null)
            textoPrincipal.text = texto;

        // Se houver subtítulo ou legenda opcional, você pode tratar aqui se necessário
        if (textoSubtitulo != null)
            textoSubtitulo.gameObject.SetActive(false); // Oculta subtítulo se a pergunta usar texto simples
    }

    public void DefinirSelecionada(bool selecionada)
    {
        if (fundo != null)
            fundo.color = selecionada ? corSelecionada : corNormal;

        if (indicadorAtivo != null)
            indicadorAtivo.SetActive(selecionada);

        if (textoPrincipal != null)
            textoPrincipal.color = selecionada ? rosa : branco;
        
        if (textoSubtitulo != null)
            textoSubtitulo.color = lilas;
    }
}