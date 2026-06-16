using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Medidores (Sliders)")]
    public Slider sliderDopamina;
    public Slider sliderSanidade;
    public Slider sliderControle;
    public Slider sliderFissura;
    public Slider sliderPercepcao;
    public Slider sliderAtencao;

    [Header("Textos e Inputs")]
    public TMP_Text textoSaldo;
    public TMP_Text textoMensagem;
    public TMP_InputField inputValorAposta;

    // Variáveis de controle do jogo
    private float saldo = 100f;
    private float valorAposta = 10f;
    
    // Valores iniciais dos hormônios (Perfil Jovem)
    private float dopamina = 50f;
    private float sanidade = 70f;
    private float controle = 50f; // 50 por ser perfil Jovem
    private float fissura = 30f;
    private float percepcao = 80f;
    private float atencao = 100f;

    void Start()
    {
        // Define o valor padrão no campo de texto de aposta
        if (inputValorAposta != null)
        {
            inputValorAposta.text = valorAposta.ToString();
        }
        AtualizarInterface();
        textoMensagem.text = "BASTIDORES DA BET: O algoritmo está pronto. Defina sua aposta e clique em GIRAR.";
    }

    public void ClicarGirar()
    {
        // Ler o valor atual do InputField caso o usuário tenha digitado algo
        if (inputValorAposta != null && float.TryParse(inputValorAposta.text, out float valorDigitado))
        {
            valorAposta = valorDigitado;
        }

        if (saldo >= valorAposta && valorAposta > 0)
        {
            saldo -= valorAposta;

            // Sistema de Sorteio (RNG)
            // 10% de chance de ganhar, 30% de quase ganhar, 60% de perder
            int sorteio = Random.Range(0, 100);

            if (sorteio < 10) // GANHOU
            {
                saldo += (valorAposta * 2);
                textoMensagem.text = "Você GANHOU! O cérebro recebe uma descarga massiva de Dopamina, gerando uma falsa ilusão de controle.";
                
                dopamina += 30;
                sanidade += 5;
                controle -= 2 * 1.5f; // Multiplicador de 1.5x do perfil jovem
                fissura += 10;
                percepcao -= 3 * 1.5f;
            }
            else if (sorteio < 40) // QUASE GANHOU
            {
                textoMensagem.text = "QUASE! Faltou só um símbolo. Psicologicamente, o 'quase ganhar' estimula o cérebro tanto quanto a vitória, induzindo você a continuar jogando.";
                
                dopamina += 20;
                sanidade -= 12 * 1.5f;
                controle -= 5 * 1.5f;
                fissura += 25;
                percepcao -= 8 * 1.5f;
            }
            else // PERDEU
            {
                textoMensagem.text = "Você PERDEU. A perda ativa áreas de frustração no cérebro, mas a Fissura gerada te compele a tentar recuperar o dinheiro.";
                
                dopamina -= 15;
                sanidade -= 5 * 1.5f;
                controle += 3;
                fissura += 15;
                percepcao -= 5 * 1.5f;
            }

            // Limita as barras entre 0 e 100 para não estourarem o visual
            LimitarValores();
            AtualizarInterface();
        }
        else
        {
            textoMensagem.text = "AVISO: Saldo insuficiente ou aposta inválida. Na vida real, o esgotamento de recursos gera crises de ansiedade severas.";
        }
    }

    public void ClicarDobrar()
    {
        if (inputValorAposta != null && float.TryParse(inputValorAposta.text, out float valorDigitado))
        {
            valorAposta = valorDigitado;
        }

        valorAposta *= 2;
        inputValorAposta.text = valorAposta.ToString();

        // Dobrar aposta afeta o psicológico imediatamente
        dopamina += 5;
        controle -= 8 * 1.5f;
        fissura += 20;
        percepcao -= 10 * 1.5f;

        textoMensagem.text = "Aposta Dobrada! Tentar dobrar para recuperar perdas rapidamente é um comportamento compulsivo clássico de risco.";
        
        LimitarValores();
        AtualizarInterface();
    }

    public void AumentarAposta()
    {
        valorAposta += 5f;
        inputValorAposta.text = valorAposta.ToString();
    }

    public void DiminuirAposta()
    {
        if (valorAposta > 5f)
        {
            valorAposta -= 5f;
            inputValorAposta.text = valorAposta.ToString();
        }
    }

    public void ClicarSair()
    {
        SceneManager.LoadScene("Scene_Inicial");
    }

    void LimitarValores()
    {
        dopamina = Mathf.Clamp(dopamina, 0f, 100f);
        sanidade = Mathf.Clamp(sanidade, 0f, 100f);
        controle = Mathf.Clamp(controle, 0f, 100f);
        fissura = Mathf.Clamp(fissura, 0f, 100f);
        percepcao = Mathf.Clamp(percepcao, 0f, 100f);
        atencao = Mathf.Clamp(atencao, 0f, 100f);
    }

    void AtualizarInterface()
    {
        // Atualiza os sliders na tela (dividido por 100 porque o slider vai de 0 a 1)
        if(sliderDopamina) sliderDopamina.value = dopamina / 100f;
        if(sliderSanidade) sliderSanidade.value = sanidade / 100f;
        if(sliderControle) sliderControle.value = controle / 100f;
        if(sliderFissura) sliderFissura.value = fissura / 100f;
        if(sliderPercepcao) sliderPercepcao.value = percepcao / 100f;
        if(sliderAtencao) sliderAtencao.value = atencao / 100f;

        if(textoSaldo) textoSaldo.text = "SALDO: R$ " + saldo.ToString("F2");
    }
}