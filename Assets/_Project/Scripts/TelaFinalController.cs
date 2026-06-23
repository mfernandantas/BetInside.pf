using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaFinalController : MonoBehaviour
{
    [Header("Componentes de Texto da Interface")]
    public TextMeshProUGUI txtPerfil;
    public TextMeshProUGUI txtSaldoFinal;
    public TextMeshProUGUI txtMedidoresFinais; // Caixa de texto grande para listar o relatório biológico
    public TextMeshProUGUI txtMensagemEducativa;

    void Start()
    {
        // Garante que o mouse vai aparecer e funcionar na tela de menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Recupera os dados que salvamos no GameManager
        string perfil = PlayerPrefs.GetString("PerfilUsado", "JOVEM");
        float saldo = PlayerPrefs.GetFloat("SaldoFinal", 0f);
        
        float dopa = PlayerPrefs.GetFloat("DopaminaFinal", 50f);
        float sani = PlayerPrefs.GetFloat("SanidadeFinal", 70f);
        float cont = PlayerPrefs.GetFloat("ControleFinal", 50f);
        float fiss = PlayerPrefs.GetFloat("FissuraFinal", 30f);
        float perc = PlayerPrefs.GetFloat("PercepcaoFinal", 80f);
        float aten = PlayerPrefs.GetFloat("AtencaoFinal", 100f);

        // Altera os textos principais da tela
        txtPerfil.text = "PERFIL ANALISADO: " + perfil.ToUpper();
        txtSaldoFinal.text = "SALDO FINAL: R$ " + saldo.ToString("F2");

        // Monta a lista com o estado da saúde mental do personagem
        txtMedidoresFinais.text = "ESTADO BIOLÓGICO FINAL:\n" +
                                 "• Dopamina: " + dopa.ToString("F0") + "%\n" +
                                 "• Sanidade: " + sani.ToString("F0") + "%\n" +
                                 "• Controle de Impulso: " + cont.ToString("F0") + "%\n" +
                                 "• Fissura por Jogo: " + fiss.ToString("F0") + "%\n" +
                                 "• Percepção da Realidade: " + perc.ToString("F0") + "%\n" +
                                 "• Atenção Externa: " + aten.ToString("F0") + "%";

        // Gera o diagnóstico baseado no estrago financeiro
        if (saldo <= 0)
        {
            txtMensagemEducativa.text = "DIAGNÓSTICO: O ALGORITMO VENCEU.\n\nA busca compulsiva por recuperar o dinheiro liberou picos artificiais de dopamina, destruindo o controle de impulso do indivíduo. Na vida real, as apostas alteram a química do cérebro de forma idêntica a dependências químicas graves.";
        }
        else
        {
            txtMensagemEducativa.text = "DIAGNÓSTICO: CICLO DE DEPENDÊNCIA ATIVO.\n\nMesmo restando saldo, os níveis hormonais indicam alta vulnerabilidade. O cérebro reage a 'quase vitórias' da mesma forma que a ganhos reais, estimulando a insistência e preparando o terreno para a perda total subsequente.";
        }
    }

    // Função para o botão NOVA SIMULAÇÃO
    public void ReiniciarSimulacao()
    {
        SceneManager.LoadScene("Scene_Inicial");
    }

    // Função para o botão SAIR
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("O jogo fechou com sucesso.");
    }
}