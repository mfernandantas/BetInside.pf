using UnityEngine;
using UnityEngine.UI;

public class OnboardingManager : MonoBehaviour
{
    [Header("Paineis da UI")]
    public GameObject cardOQueSaoBets;
    public GameObject painelQuestionario;

    [Header("Opcoes do Questionario")]
    public OpcaoQuestionario[] opcoes;

    [Header("Botao Proxima")]
    public Button botaoProxima;
    public Image imagemBotaoProxima;

    [Header("Cores")]
    public Color corBotaoAtivo = new Color32(255, 45, 85, 255);
    public Color corBotaoInativo = new Color32(32, 34, 48, 255);

    private int opcaoSelecionada = -1;

    private void Start()
    {
        DesativarBotaoProxima();
    }

    // Chamado pelo botao "O que sao bets?"
    public void AbrirCardInfo()
    {
        if (cardOQueSaoBets != null)
            cardOQueSaoBets.SetActive(true);
    }

    // Chamado pelo botao "ENTENDI" dentro do Card
    public void FecharCardInfo()
    {
        if (cardOQueSaoBets != null)
            cardOQueSaoBets.SetActive(false);
    }

    // Chamado pelo botao "INICIAR SIMULACAO"
    public void AvancarParaQuestionario()
    {
        if (cardOQueSaoBets != null)
            cardOQueSaoBets.SetActive(false);

        if (painelQuestionario != null)
            painelQuestionario.SetActive(true);
    }

    // Chamado quando o usuario clica em uma opcao
    public void SelecionarOpcao(int indice)
    {
        opcaoSelecionada = indice;

        for (int i = 0; i < opcoes.Length; i++)
        {
            opcoes[i].DefinirSelecionada(i == indice);
        }

        AtivarBotaoProxima();
    }

    private void AtivarBotaoProxima()
    {
        if (botaoProxima != null)
            botaoProxima.interactable = true;

        if (imagemBotaoProxima != null)
            imagemBotaoProxima.color = corBotaoAtivo;
    }

    private void DesativarBotaoProxima()
    {
        if (botaoProxima != null)
            botaoProxima.interactable = false;

        if (imagemBotaoProxima != null)
            imagemBotaoProxima.color = corBotaoInativo;
    }
}