
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

    [Header("Textos dos Medidores (Numeros)")]
    public TMP_Text textoDopamina;
    public TMP_Text textoSanidade;
    public TMP_Text textoControle;
    public TMP_Text textoFissura;
    public TMP_Text textoPercepcao;
    public TMP_Text textoAtencao;

    [Header("Cores dos Estados")]
    private Color corVerde = new Color(0f, 0.96f, 0.83f);
    private Color corAmarela = new Color(1f, 0.82f, 0.4f);
    private Color corVermelha = new Color(1f, 0.18f, 0.33f);

    [Header("Animacao das Barras")]
    [SerializeField] private float velocidadeBarras = 0.30f;

    [Header("Textos e Inputs")]
    public TMP_Text textoSaldo;
    public TMP_Text textoMensagem;
    public TMP_InputField inputValorAposta;

    [Header("Texto da Aposta Atual")]
    public TMP_Text textoAposta;

    [Header("Rolinhos")]
    public Image slot1;
    public Image slot2;
    public Image slot3;

    [Header("Simbolos dos Rolinhos")]
    public Sprite[] simbolos;

    [Header("Botao Girar")]
    public Button botaoGirar;

    // Controle do jogo
    private float saldo = 500f;
    private float valorAposta = 10f;
    private bool girando = false;

    // Valores iniciais dos medidores
    private float dopamina = 30f;
    private float sanidade = 30f;
    private float controle = 30f;
    private float fissura = 20f;
    private float percepcao = 30f;
    private float atencao = 30f;

    // Valores visuais das barras
    private float visualDopamina;
    private float visualSanidade;
    private float visualControle;
    private float visualFissura;
    private float visualPercepcao;
    private float visualAtencao;

    void Start()
    {
        visualDopamina = dopamina;
        visualSanidade = sanidade;
        visualControle = controle;
        visualFissura = fissura;
        visualPercepcao = percepcao;
        visualAtencao = atencao;

        AtualizarBarrasImediatamente();
        AtualizarSaldo();
        AtualizarAposta();

        if (textoMensagem != null)
        {
            textoMensagem.text =
                "BASTIDORES DA BET: Voce comeca com R$ 500,00 ficticios. Observe como cada giro afeta seus medidores cerebrais.";
        }
    }

    void Update()
    {
        AnimarBarras();
    }

    // ANIMACAO DAS BARRAS

    void AnimarBarras()
    {
        float passo = velocidadeBarras * 100f * Time.deltaTime;

        visualDopamina = Mathf.MoveTowards(visualDopamina, dopamina, passo);
        visualSanidade = Mathf.MoveTowards(visualSanidade, sanidade, passo);
        visualControle = Mathf.MoveTowards(visualControle, controle, passo);
        visualFissura = Mathf.MoveTowards(visualFissura, fissura, passo);
        visualPercepcao = Mathf.MoveTowards(visualPercepcao, percepcao, passo);
        visualAtencao = Mathf.MoveTowards(visualAtencao, atencao, passo);

        AtualizarSliderComCor(sliderDopamina, textoDopamina, visualDopamina);
        AtualizarSliderComCor(sliderSanidade, textoSanidade, visualSanidade);
        AtualizarSliderComCor(sliderControle, textoControle, visualControle);
        AtualizarSliderComCor(sliderFissura, textoFissura, visualFissura);
        AtualizarSliderComCor(sliderPercepcao, textoPercepcao, visualPercepcao);
        AtualizarSliderComCor(sliderAtencao, textoAtencao, visualAtencao);
    }

    void AtualizarSliderComCor(Slider slider, TMP_Text texto, float valor)
    {
        if (slider != null)
            slider.value = valor / 100f;

        if (texto != null)
        {
            texto.text = Mathf.RoundToInt(valor).ToString();

            if (valor >= 70f)
            {
                texto.color = corVermelha;

                if (slider != null && slider.fillRect != null)
                    slider.fillRect.GetComponent<Image>().color = corVermelha;
            }
            else if (valor >= 45f)
            {
                texto.color = corAmarela;

                if (slider != null && slider.fillRect != null)
                    slider.fillRect.GetComponent<Image>().color = corAmarela;
            }
            else
            {
                texto.color = corVerde;

                if (slider != null && slider.fillRect != null)
                    slider.fillRect.GetComponent<Image>().color = corVerde;
            }
        }
    }

    void AtualizarBarrasImediatamente()
    {
        AtualizarSliderComCor(sliderDopamina, textoDopamina, visualDopamina);
        AtualizarSliderComCor(sliderSanidade, textoSanidade, visualSanidade);
        AtualizarSliderComCor(sliderControle, textoControle, visualControle);
        AtualizarSliderComCor(sliderFissura, textoFissura, visualFissura);
        AtualizarSliderComCor(sliderPercepcao, textoPercepcao, visualPercepcao);
        AtualizarSliderComCor(sliderAtencao, textoAtencao, visualAtencao);
    }

    // GIRAR

    public void ClicarGirar()
    {
        if (girando)
            return;

        if (saldo >= valorAposta && valorAposta > 0)
        {
            if (!RolinhosConfigurados())
                return;

            saldo -= valorAposta;
            AtualizarSaldo();

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
                textoMensagem.text = "Saldo insuficiente para essa aposta. Diminua o valor.";
            }
        }
    }

    // VITORIA, QUASE VITORIA E PERDA

    void PrepararVitoria()
    {
        if (!RolinhosConfigurados()) return;

        int escolhido = Random.Range(0, simbolos.Length);

        StartCoroutine(AnimarRolinhos(
            simbolos[escolhido],
            simbolos[escolhido],
            simbolos[escolhido],
            0
        ));
    }

    void PrepararQuase()
    {
        if (!RolinhosConfigurados()) return;

        int simboloIgual = Random.Range(0, simbolos.Length);
        int simboloDiferente;

        do
        {
            simboloDiferente = Random.Range(0, simbolos.Length);
        }
        while (simboloDiferente == simboloIgual);

        int posicaoDiferente = Random.Range(0, 3);

        Sprite r1 = posicaoDiferente == 0
            ? simbolos[simboloDiferente]
            : simbolos[simboloIgual];

        Sprite r2 = posicaoDiferente == 1
            ? simbolos[simboloDiferente]
            : simbolos[simboloIgual];

        Sprite r3 = posicaoDiferente == 2
            ? simbolos[simboloDiferente]
            : simbolos[simboloIgual];

        StartCoroutine(AnimarRolinhos(r1, r2, r3, 1));
    }

    void PrepararPerda()
    {
        if (!RolinhosConfigurados()) return;

        int p1 = Random.Range(0, simbolos.Length);
        int p2, p3;

        do
        {
            p2 = Random.Range(0, simbolos.Length);
        }
        while (p2 == p1);

        do
        {
            p3 = Random.Range(0, simbolos.Length);
        }
        while (p3 == p1 || p3 == p2);

        StartCoroutine(AnimarRolinhos(
            simbolos[p1],
            simbolos[p2],
            simbolos[p3],
            2
        ));
    }

    // ANIMACAO DOS ROLINHOS

    IEnumerator AnimarRolinhos(
        Sprite resultado1,
        Sprite resultado2,
        Sprite resultado3,
        int tipoResultado
    )
    {
        girando = true;

        if (botaoGirar != null)
            botaoGirar.interactable = false;

        if (textoMensagem != null)
            textoMensagem.text = "Girando...";

        float velocidade = 0.06f;

        for (int i = 0; i < 5; i++)
        {
            slot3.sprite = simbolos[Random.Range(0, simbolos.Length)];
            yield return new WaitForSeconds(velocidade);
        }

        for (int i = 0; i < 5; i++)
        {
            slot3.sprite = simbolos[Random.Range(0, simbolos.Length)];
            slot2.sprite = simbolos[Random.Range(0, simbolos.Length)];
            yield return new WaitForSeconds(velocidade);
        }

        for (int i = 0; i < 7; i++)
        {
            slot3.sprite = simbolos[Random.Range(0, simbolos.Length)];
            slot2.sprite = simbolos[Random.Range(0, simbolos.Length)];
            slot1.sprite = simbolos[Random.Range(0, simbolos.Length)];
            yield return new WaitForSeconds(velocidade);
        }

        slot3.sprite = resultado3;

        for (int i = 0; i < 5; i++)
        {
            slot2.sprite = simbolos[Random.Range(0, simbolos.Length)];
            slot1.sprite = simbolos[Random.Range(0, simbolos.Length)];
            yield return new WaitForSeconds(velocidade);
        }

        slot2.sprite = resultado2;

        for (int i = 0; i < 5; i++)
        {
            slot1.sprite = simbolos[Random.Range(0, simbolos.Length)];
            yield return new WaitForSeconds(velocidade);
        }

        slot1.sprite = resultado1;

        yield return new WaitForSeconds(0.25f);

        AplicarResultado(tipoResultado);

        girando = false;

        if (botaoGirar != null)
            botaoGirar.interactable = true;
    }

    // APLICAR RESULTADO

    void AplicarResultado(int tipoResultado)
    {
        if (tipoResultado == 0)
        {
            saldo += valorAposta * 2;

            if (textoMensagem != null)
                textoMensagem.text =
                    "Voce GANHOU! Observe como uma vitoria pode aumentar a vontade de continuar.";

            dopamina += 30;
            sanidade += 5;
            controle -= 3f;
            fissura += 10;
            percepcao -= 4.5f;
        }
        else if (tipoResultado == 1)
        {
            if (textoMensagem != null)
                textoMensagem.text =
                    "QUASE! Dois simbolos ficaram iguais. Esse tipo de resultado pode incentivar novas tentativas.";

            dopamina += 20;
            sanidade -= 18f;
            controle -= 7.5f;
            fissura += 25;
            percepcao -= 12f;
        }
        else
        {
            if (textoMensagem != null)
                textoMensagem.text =
                    "Voce PERDEU. Observe como a frustracao da perda pode estimular a tentativa de recuperar o valor.";

            dopamina -= 15;
            sanidade -= 7.5f;
            controle += 3;
            fissura += 15;
            percepcao -= 7.5f;
        }

        LimitarValores();
        AtualizarSaldo();

        if (saldo <= 0)
        {
            saldo = 0;
            AtualizarSaldo();
            StartCoroutine(FinalizarComEspera());
        }
    }

    IEnumerator FinalizarComEspera()
    {
        yield return new WaitForSeconds(1.5f);
        FinalizarSimulacao();
    }

    bool RolinhosConfigurados()
    {
        if (slot1 == null ||
            slot2 == null ||
            slot3 == null ||
            simbolos == null ||
            simbolos.Length < 3)
        {
            Debug.LogWarning(
                "Configure os rolinhos e simbolos no GameManager."
            );

            return false;
        }

        return true;
    }

    // DOBRAR APOSTA

    public void ClicarDobrar()
    {
        if (girando) return;

        float novoValor = valorAposta * 2f;

        if (novoValor > saldo)
        {
            if (textoMensagem != null)
                textoMensagem.text =
                    "Saldo insuficiente para dobrar a aposta.";

            return;
        }

        valorAposta = novoValor;
        AtualizarAposta();

        dopamina += 5;
        controle -= 12f;
        fissura += 20;
        percepcao -= 15f;

        if (textoMensagem != null)
            textoMensagem.text =
                "Aposta dobrada. Aumentar valores para recuperar perdas e um comportamento de risco.";

        LimitarValores();
    }

    // AUMENTAR APOSTA

    public void AumentarAposta()
    {
        if (girando) return;

        if (valorAposta + 5f <= saldo)
        {
            valorAposta += 5f;
            AtualizarAposta();
        }
        else if (textoMensagem != null)
        {
            textoMensagem.text =
                "Saldo insuficiente para aumentar a aposta.";
        }
    }

    // DIMINUIR APOSTA

    public void DiminuirAposta()
    {
        if (girando) return;

        if (valorAposta > 5f)
        {
            valorAposta -= 5f;
            AtualizarAposta();
        }
    }

    // ATUALIZAR APOSTA NA TELA

    void AtualizarAposta()
    {
        if (textoAposta != null)
        {
            textoAposta.text =
                "APOSTA: R$ " + valorAposta.ToString("F2");
        }
    }

    // ATUALIZAR SALDO

    void AtualizarSaldo()
    {
        if (textoSaldo != null)
        {
            textoSaldo.text =
                "SALDO: R$ " + saldo.ToString("F2");
        }
    }

    // SAIR

    public void ClicarSair()
    {
        SceneManager.LoadScene("Scene_Inicial");
    }

    // LIMITAR MEDIDORES

    void LimitarValores()
    {
        dopamina = Mathf.Clamp(dopamina, 0f, 100f);
        sanidade = Mathf.Clamp(sanidade, 0f, 100f);
        controle = Mathf.Clamp(controle, 0f, 100f);
        fissura = Mathf.Clamp(fissura, 0f, 100f);
        percepcao = Mathf.Clamp(percepcao, 0f, 100f);
        atencao = Mathf.Clamp(atencao, 0f, 100f);
    }

    // FINALIZAR SIMULACAO

    public void FinalizarSimulacao()
    {
        PlayerPrefs.SetFloat("SaldoFinal", saldo);
        PlayerPrefs.SetFloat("DopaminaFinal", dopamina);
        PlayerPrefs.SetFloat("SanidadeFinal", sanidade);
        PlayerPrefs.SetFloat("ControleFinal", controle);
        PlayerPrefs.SetFloat("FissuraFinal", fissura);
        PlayerPrefs.SetFloat("PercepcaoFinal", percepcao);
        PlayerPrefs.SetFloat("AtencaoFinal", atencao);

        string perfilAtual =
            PlayerPrefs.GetString("PerfilUsuario", "Jovem");

        PlayerPrefs.SetString("PerfilUsado", perfilAtual);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Scene_Final");
    }
}
