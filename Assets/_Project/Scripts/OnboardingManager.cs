using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OnboardingManager : MonoBehaviour
{
    [System.Serializable]
    public struct PerguntaData
    {
        [TextArea] public string tituloPergunta;
        [Multiline] public string[] opcoesTexto;
    }

    [Header("Paineis da UI")]
    public GameObject panelCapa;
    public GameObject cardOQueSaoBets;
    public GameObject painelQuestionario;

    [Header("Elementos do Questionario")]
    public TMP_Text textoPasso;
    public TMP_Text textoPorcentagem;
    public Image progressBarFill;
    public TMP_Text textoPergunta;
    public OpcaoQuestionario[] opcoesUI;

    [Header("Botoes de Navegacao")]
    public Button botaoVoltar;
    public Button botaoProxima;
    public TMP_Text textoBotaoProxima;
    public Image imagemBotaoProxima;

    [Header("Cores")]
    public Color corBotaoAtivo = new Color32(255, 45, 85, 255);
    public Color corBotaoInativo = new Color32(32, 34, 48, 255);

    [Header("Lista de Perguntas da Pesquisa")]
    public PerguntaData[] perguntas;

    private int indiceAtual = 0;
    private int[] respostasUsuario;
    private int opcaoSelecionadaAtual = -1;

    private void Start()
    {
        if (perguntas != null && perguntas.Length > 0)
        {
            respostasUsuario = new int[perguntas.Length];
            for (int i = 0; i < respostasUsuario.Length; i++) respostasUsuario[i] = -1;
        }

        DesativarBotaoProxima();
        AtualizarInterfacePergunta();
    }

    public void AbrirCardInfo()
    {
        if (cardOQueSaoBets != null) cardOQueSaoBets.SetActive(true);
    }

    public void FecharCardInfo()
    {
        if (cardOQueSaoBets != null) cardOQueSaoBets.SetActive(false);
    }

    public void AvancarParaQuestionario()
    {
        if (cardOQueSaoBets != null) cardOQueSaoBets.SetActive(false);
        if (panelCapa != null) panelCapa.SetActive(false);
        if (painelQuestionario != null) painelQuestionario.SetActive(true);

        AtualizarInterfacePergunta();
    }

    public void SelecionarOpcao(int indiceOpcao)
    {
        opcaoSelecionadaAtual = indiceOpcao;
        respostasUsuario[indiceAtual] = indiceOpcao;

        for (int i = 0; i < opcoesUI.Length; i++)
        {
            if (opcoesUI[i] != null)
                opcoesUI[i].DefinirSelecionada(i == indiceOpcao);
        }

        AtivarBotaoProxima();
    }

    public void BotaoProximaClicado()
    {
        if (opcaoSelecionadaAtual == -1) return;

        // Se for a última pergunta, finaliza e vai para o jogo
        if (indiceAtual >= perguntas.Length - 1)
        {
            SalvarRespostasEIniciarJogo();
            return;
        }

        // Vai para a próxima pergunta
        indiceAtual++;
        AtualizarInterfacePergunta();
    }

    public void BotaoVoltarClicado()
    {
        if (indiceAtual > 0)
        {
            indiceAtual--;
            AtualizarInterfacePergunta();
        }
        else
        {
            // Se estiver na primeira pergunta, volta para a capa
            if (painelQuestionario != null) painelQuestionario.SetActive(false);
            if (panelCapa != null) panelCapa.SetActive(true);
        }
    }

    private void AtualizarInterfacePergunta()
    {
        if (perguntas == null || perguntas.Length == 0) return;

        PerguntaData pAtual = perguntas[indiceAtual];

        // Atualizar Texto da Pergunta
        if (textoPergunta != null)
            textoPergunta.text = pAtual.tituloPergunta;

        // Atualizar Textos das Opções
        for (int i = 0; i < opcoesUI.Length; i++)
        {
            if (i < pAtual.opcoesTexto.Length)
            {
                opcoesUI[i].gameObject.SetActive(true);
                opcoesUI[i].AtualizarTexto(pAtual.opcoesTexto[i]);
            }
            else
            {
                opcoesUI[i].gameObject.SetActive(false);
            }
        }

        // Restaurar seleção anterior se já respondeu
        opcaoSelecionadaAtual = respostasUsuario[indiceAtual];
        for (int i = 0; i < opcoesUI.Length; i++)
        {
            if (opcoesUI[i] != null)
                opcoesUI[i].DefinirSelecionada(i == opcaoSelecionadaAtual);
        }

        if (opcaoSelecionadaAtual != -1) AtivarBotaoProxima();
        else DesativarBotaoProxima();

        // Atualizar Barra de Progresso e Indicadores (01 / 05)
        int passoAtual = indiceAtual + 1;
        int totalPassos = perguntas.Length;

        if (textoPasso != null)
            textoPasso.text = passoAtual.ToString("00") + " / " + totalPassos.ToString("00");

        float porcentagem = ((float)passoAtual / totalPassos) * 100f;
        if (textoPorcentagem != null)
            textoPorcentagem.text = Mathf.RoundToInt(porcentagem) + "%";

        if (progressBarFill != null)
            progressBarFill.fillAmount = (float)passoAtual / totalPassos;

        // Ajustar texto do botão Próxima se for a última
        if (textoBotaoProxima != null)
        {
            if (indiceAtual == totalPassos - 1)
                textoBotaoProxima.text = "FINALIZAR >";
            else
                textoBotaoProxima.text = "PRÓXIMA >";
        }
    }

    private void AtivarBotaoProxima()
    {
        if (botaoProxima != null) botaoProxima.interactable = true;
        if (imagemBotaoProxima != null) imagemBotaoProxima.color = corBotaoAtivo;
    }

    private void DesativarBotaoProxima()
    {
        if (botaoProxima != null) botaoProxima.interactable = false;
        if (imagemBotaoProxima != null) imagemBotaoProxima.color = corBotaoInativo;
    }

    private void SalvarRespostasEIniciarJogo()
    {
        // Salva a faixa etaria (pergunta 1) como perfil principal para a cena do jogo
        if (respostasUsuario[0] != -1)
        {
            string perfilStr = "Jovem";
            if (respostasUsuario[0] == 0) perfilStr = "Adolescente (12-17)";
            else if (respostasUsuario[0] == 1) perfilStr = "Jovem Adulto (18-24)";
            else if (respostasUsuario[0] == 2) perfilStr = "Adulto (25-39)";
            else perfilStr = "Adulto (+40)";

            PlayerPrefs.SetString("PerfilUsuario", perfilStr);
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene("Scene_Jogo");
    }
}