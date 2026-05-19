using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configurações Globais")]
    public int perfilSelecionado; // 0 = Jovem, 1 = Adulto
    public int faixaEtariaIndex;   
    public float saldoAtual = 200f;
    public float tempoDeJogo = 0f;

    [Header("Medidores Psicológicos")]
    public float dopamina;
    public float sanidade;
    public float controleImpulso;
    public float fissura;
    public float percepcaoRealidade;
    public float atencaoFoco;

    private void Awake()
    {
        // Garante que só vai existir um GameManager no jogo todo e que ele não vai sumir entre as telas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetarDadosIniciais();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetarDadosIniciais()
    {
        saldoAtual = 200f; 
        tempoDeJogo = 0f;
        
        // Valores padrões descritos no planejamento pedagógico
        dopamina = 50f;
        sanidade = 70f;
        fissura = 30f;
        percepcaoRealidade = 80f;
        atencaoFoco = 100f;
        
        // O controle de impulso inicial muda dependendo do perfil selecionado
        controleImpulso = (perfilSelecionado == 0) ? 50f : 70f;
    }

    public void SalvarDados()
    {
        PlayerPrefs.SetInt("Perfil", perfilSelecionado);
        PlayerPrefs.SetInt("FaixaEtaria", faixaEtariaIndex);
        PlayerPrefs.SetFloat("Saldo", saldoAtual);
        PlayerPrefs.SetFloat("TempoJogo", tempoDeJogo);
        PlayerPrefs.SetFloat("Dopamina", dopamina);
        PlayerPrefs.SetFloat("Sanidade", sanidade);
        PlayerPrefs.SetFloat("ControleImpulso", controleImpulso);
        PlayerPrefs.SetFloat("Fissura", fissura);
        PlayerPrefs.SetFloat("PercepcaoRealidade", percepcaoRealidade);
        PlayerPrefs.SetFloat("AtencaoFoco", atencaoFoco);
        PlayerPrefs.Save();
    }

    public void CarregarDados()
    {
        perfilSelecionado = PlayerPrefs.GetInt("Perfil", 0);
        faixaEtariaIndex = PlayerPrefs.GetInt("FaixaEtaria", 0);
        saldoAtual = PlayerPrefs.GetFloat("Saldo", 200f);
        tempoDeJogo = PlayerPrefs.GetFloat("TempoJogo", 0f);
        dopamina = PlayerPrefs.GetFloat("Dopamina", 50f);
        sanidade = PlayerPrefs.GetFloat("Sanidade", 70f);
        controleImpulso = PlayerPrefs.GetFloat("ControleImpulso", 60f);
        fissura = PlayerPrefs.GetFloat("Fissura", 30f);
        percepcaoRealidade = PlayerPrefs.GetFloat("PercepcaoRealidade", 80f);
        atencaoFoco = PlayerPrefs.GetFloat("AtencaoFoco", 100f);
    }

    public void MudarCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }
}