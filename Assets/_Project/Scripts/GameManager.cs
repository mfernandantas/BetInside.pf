using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Rolinhos")]
    public Image slot1;
    public Image slot2;
    public Image slot3;

    [Header("Simbolos dos Rolinhos")]
    public Sprite[] simbolos;

    [Header("Botao Girar")]
    public Button botaoGirar;

    // Variáveis de controle
    private float saldo = 100f;
    private float valorAposta = 10f;
    private bool girando = false;

    // Valores iniciais
    private float dopamina = 50f;
    private float sanidade = 70f;
    private float controle = 50f;
    private float fissura = 30f;
    private float percepcao = 80f;
    private float atencao = 100f;

    void Start()
    {
        if (inputValorAposta != null)
        {
            inputValorAposta.text = valorAposta.ToString();
        }

        AtualizarInterface();

        if (textoMensagem != null)
        {
            textoMensagem.text =
                "BASTIDORES DA BET: O algoritmo está pronto. Defina sua aposta e clique em GIRAR.";
        }
    }

    public void ClicarGirar()
    {
        // Impede vários giros ao mesmo tempo
        if (girando)
            return;

        // Lê o valor digitado
        if (inputValorAposta != null &&
            float.TryParse(inputValorAposta.text, out float valorDigitado))
        {
            valorAposta = valorDigitado;
        }

        if (saldo >= valorAposta && valorAposta > 0)
        {
            saldo -= valorAposta;

            AtualizarInterface();

            // Sorteio da simulação
            // 10% vitória
            // 30% quase
            // 60% perda
            int sorteio = Random.Range(0, 100);

            if (sorteio < 10)
            {
                PrepararVitoria();
            }
            else if (sorteio < 40)
            {
                PrepararQuase();
            }
            else
            {
                PrepararPerda();
            }
        }
        else
        {
            if (saldo <= 0)
            {
                FinalizarSimulacao();
            }
            else if (textoMensagem != null)
            {
                textoMensagem.text =
                    "Saldo insuficiente ou valor inválido.";
            }
        }
    }

    // =====================================================
    // PREPARAR VITÓRIA
    // =====================================================

    void PrepararVitoria()
    {
        if (!RolinhosConfigurados())
            return;

        int escolhido = Random.Range(0, simbolos.Length);

        Sprite resultado1 = simbolos[escolhido];
        Sprite resultado2 = simbolos[escolhido];
        Sprite resultado3 = simbolos[escolhido];

        StartCoroutine(
            AnimarRolinhos(
                resultado1,
                resultado2,
                resultado3,
                0
            )
        );
    }

    // =====================================================
    // PREPARAR QUASE
    // =====================================================

    void PrepararQuase()
    {
        if (!RolinhosConfigurados())
            return;

        int simboloIgual =
            Random.Range(0, simbolos.Length);

        int simboloDiferente;

        do
        {
            simboloDiferente =
                Random.Range(0, simbolos.Length);
        }
        while (simboloDiferente == simboloIgual);

        int posicaoDiferente =
            Random.Range(0, 3);

        Sprite resultado1 =
            posicaoDiferente == 0
            ? simbolos[simboloDiferente]
            : simbolos[simboloIgual];

        Sprite resultado2 =
            posicaoDiferente == 1
            ? simbolos[simboloDiferente]
            : simbolos[simboloIgual];

        Sprite resultado3 =
            posicaoDiferente == 2
            ? simbolos[simboloDiferente]
            : simbolos[simboloIgual];

        StartCoroutine(
            AnimarRolinhos(
                resultado1,
                resultado2,
                resultado3,
                1
            )
        );
    }

    // =====================================================
    // PREPARAR PERDA
    // =====================================================

    void PrepararPerda()
    {
        if (!RolinhosConfigurados())
            return;

        int primeiro =
            Random.Range(0, simbolos.Length);

        int segundo;

        do
        {
            segundo =
                Random.Range(0, simbolos.Length);
        }
        while (segundo == primeiro);

        int terceiro;

        do
        {
            terceiro =
                Random.Range(0, simbolos.Length);
        }
        while (
            terceiro == primeiro ||
            terceiro == segundo
        );

        Sprite resultado1 = simbolos[primeiro];
        Sprite resultado2 = simbolos[segundo];
        Sprite resultado3 = simbolos[terceiro];

        StartCoroutine(
            AnimarRolinhos(
                resultado1,
                resultado2,
                resultado3,
                2
            )
        );
    }

    // =====================================================
    // ANIMAÇÃO DOS ROLINHOS
    //
    // NO SEU LAYOUT:
    // slot3 = ESQUERDA
    // slot2 = MEIO
    // slot1 = DIREITA
    //
    // TODOS usam a mesma velocidade
    // =====================================================

    IEnumerator AnimarRolinhos(
        Sprite resultado1,
        Sprite resultado2,
        Sprite resultado3,
        int tipoResultado)
    {
        girando = true;

        if (botaoGirar != null)
        {
            botaoGirar.interactable = false;
        }

        if (textoMensagem != null)
        {
            textoMensagem.text = "Girando...";
        }

        // MESMA velocidade para todos
        float velocidade = 0.06f;

        // =================================================
        // 1. ESQUERDA COMEÇA
        // slot3
        // =================================================

        for (int i = 0; i < 5; i++)
        {
            slot3.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            yield return new WaitForSeconds(velocidade);
        }

        // =================================================
        // 2. MEIO COMEÇA
        // esquerda + meio girando
        // =================================================

        for (int i = 0; i < 5; i++)
        {
            slot3.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            slot2.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            yield return new WaitForSeconds(velocidade);
        }

        // =================================================
        // 3. DIREITA COMEÇA
        // os três giram juntos
        // =================================================

        for (int i = 0; i < 7; i++)
        {
            slot3.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            slot2.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            slot1.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            yield return new WaitForSeconds(velocidade);
        }

        // =================================================
        // 4. ESQUERDA PARA
        // =================================================

        slot3.sprite = resultado3;

        // Meio e direita continuam na MESMA velocidade
        for (int i = 0; i < 5; i++)
        {
            slot2.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            slot1.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            yield return new WaitForSeconds(velocidade);
        }

        // =================================================
        // 5. MEIO PARA
        // =================================================

        slot2.sprite = resultado2;

        // Direita continua na MESMA velocidade
        for (int i = 0; i < 5; i++)
        {
            slot1.sprite =
                simbolos[
                    Random.Range(0, simbolos.Length)
                ];

            yield return new WaitForSeconds(velocidade);
        }

        // =================================================
        // 6. DIREITA PARA
        // =================================================

        slot1.sprite = resultado1;

        yield return new WaitForSeconds(0.25f);

        AplicarResultado(tipoResultado);

        girando = false;

        if (botaoGirar != null)
        {
            botaoGirar.interactable = true;
        }
    }

    // =====================================================
    // APLICAR RESULTADO
    // =====================================================

    void AplicarResultado(int tipoResultado)
    {
        // GANHOU
        if (tipoResultado == 0)
        {
            saldo += valorAposta * 2;

            if (textoMensagem != null)
            {
                textoMensagem.text =
                    "Você GANHOU! Observe como uma vitória pode aumentar a vontade de continuar.";
            }

            dopamina += 30;
            sanidade += 5;
            controle -= 2 * 1.5f;
            fissura += 10;
            percepcao -= 3 * 1.5f;
        }

        // QUASE GANHOU
        else if (tipoResultado == 1)
        {
            if (textoMensagem != null)
            {
                textoMensagem.text =
                    "QUASE! Dois símbolos ficaram iguais. Esse tipo de resultado pode incentivar novas tentativas.";
            }

            dopamina += 20;
            sanidade -= 12 * 1.5f;
            controle -= 5 * 1.5f;
            fissura += 25;
            percepcao -= 8 * 1.5f;
        }

        // PERDEU
        else
        {
            if (textoMensagem != null)
            {
                textoMensagem.text =
                    "Você PERDEU. Observe como a frustração da perda pode estimular a tentativa de recuperar o valor.";
            }

            dopamina -= 15;
            sanidade -= 5 * 1.5f;
            controle += 3;
            fissura += 15;
            percepcao -= 5 * 1.5f;
        }

        LimitarValores();
        AtualizarInterface();

        if (saldo <= 0)
        {
            saldo = 0;

            AtualizarInterface();

            StartCoroutine(
                FinalizarComEspera()
            );
        }
    }

    // =====================================================
    // ESPERA ANTES DA TELA FINAL
    // =====================================================

    IEnumerator FinalizarComEspera()
    {
        yield return new WaitForSeconds(1.5f);

        FinalizarSimulacao();
    }

    // =====================================================
    // VERIFICAR ROLINHOS
    // =====================================================

    bool RolinhosConfigurados()
    {
        if (
            slot1 == null ||
            slot2 == null ||
            slot3 == null ||
            simbolos == null ||
            simbolos.Length < 3
        )
        {
            Debug.LogWarning(
                "Configure os rolinhos e símbolos no GameManager."
            );

            return false;
        }

        return true;
    }

    // =====================================================
    // DOBRAR
    // =====================================================

    public void ClicarDobrar()
    {
        if (girando)
            return;

        if (
            inputValorAposta != null &&
            float.TryParse(
                inputValorAposta.text,
                out float valorDigitado
            )
        )
        {
            valorAposta = valorDigitado;
        }

        valorAposta *= 2;

        if (inputValorAposta != null)
        {
            inputValorAposta.text =
                valorAposta.ToString();
        }

        dopamina += 5;
        controle -= 8 * 1.5f;
        fissura += 20;
        percepcao -= 10 * 1.5f;

        if (textoMensagem != null)
        {
            textoMensagem.text =
                "Aposta dobrada. A tentativa de recuperar perdas aumentando o valor é um comportamento de risco.";
        }

        LimitarValores();
        AtualizarInterface();
    }

    // =====================================================
    // AUMENTAR APOSTA
    // =====================================================

    public void AumentarAposta()
    {
        if (girando)
            return;

        valorAposta += 5f;

        if (inputValorAposta != null)
        {
            inputValorAposta.text =
                valorAposta.ToString();
        }
    }

    // =====================================================
    // DIMINUIR APOSTA
    // =====================================================

    public void DiminuirAposta()
    {
        if (girando)
            return;

        if (valorAposta > 5f)
        {
            valorAposta -= 5f;

            if (inputValorAposta != null)
            {
                inputValorAposta.text =
                    valorAposta.ToString();
            }
        }
    }

    // =====================================================
    // SAIR
    // =====================================================

    public void ClicarSair()
    {
        SceneManager.LoadScene(
            "Scene_Inicial"
        );
    }

    // =====================================================
    // LIMITAR MEDIDORES
    // =====================================================

    void LimitarValores()
    {
        dopamina =
            Mathf.Clamp(dopamina, 0f, 100f);

        sanidade =
            Mathf.Clamp(sanidade, 0f, 100f);

        controle =
            Mathf.Clamp(controle, 0f, 100f);

        fissura =
            Mathf.Clamp(fissura, 0f, 100f);

        percepcao =
            Mathf.Clamp(percepcao, 0f, 100f);

        atencao =
            Mathf.Clamp(atencao, 0f, 100f);
    }

    // =====================================================
    // ATUALIZAR INTERFACE
    // =====================================================

    void AtualizarInterface()
    {
        if (sliderDopamina)
        {
            sliderDopamina.value =
                dopamina / 100f;
        }

        if (sliderSanidade)
        {
            sliderSanidade.value =
                sanidade / 100f;
        }

        if (sliderControle)
        {
            sliderControle.value =
                controle / 100f;
        }

        if (sliderFissura)
        {
            sliderFissura.value =
                fissura / 100f;
        }

        if (sliderPercepcao)
        {
            sliderPercepcao.value =
                percepcao / 100f;
        }

        if (sliderAtencao)
        {
            sliderAtencao.value =
                atencao / 100f;
        }

        if (textoSaldo)
        {
            textoSaldo.text =
                "SALDO: R$ " +
                saldo.ToString("F2");
        }
    }

    // =====================================================
    // FINALIZAR SIMULAÇÃO
    // =====================================================

    public void FinalizarSimulacao()
    {
        PlayerPrefs.SetFloat(
            "SaldoFinal",
            saldo
        );

        PlayerPrefs.SetFloat(
            "DopaminaFinal",
            dopamina
        );

        PlayerPrefs.SetFloat(
            "SanidadeFinal",
            sanidade
        );

        PlayerPrefs.SetFloat(
            "ControleFinal",
            controle
        );

        PlayerPrefs.SetFloat(
            "FissuraFinal",
            fissura
        );

        PlayerPrefs.SetFloat(
            "PercepcaoFinal",
            percepcao
        );

        PlayerPrefs.SetFloat(
            "AtencaoFinal",
            atencao
        );

        string perfilAtual =
            PlayerPrefs.GetString(
                "PerfilUsuario",
                "Jovem"
            );

        PlayerPrefs.SetString(
            "PerfilUsado",
            perfilAtual
        );

        PlayerPrefs.Save();

        SceneManager.LoadScene(
            "Scene_Final"
        );
    }
}