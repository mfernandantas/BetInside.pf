using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [Header("Paineis da UI")]
    public GameObject cardOQueSaoBets;
    public GameObject painelQuestionario;

    // Chamado pelo botão "ⓘ O que são bets?"
    public void AbrirCardInfo()
    {
        if (cardOQueSaoBets != null) cardOQueSaoBets.SetActive(true);
    }

    // Chamado pelo botão "ENTENDI" dentro do Card
    public void FecharCardInfo()
    {
        if (cardOQueSaoBets != null) cardOQueSaoBets.SetActive(false);
    }

    // Chamado pelo botão "INICIAR SIMULAÇÃO"
    public void AvancarParaQuestionario()
    {
        if (cardOQueSaoBets != null) cardOQueSaoBets.SetActive(false);
        if (painelQuestionario != null) painelQuestionario.SetActive(true);
    }
}