using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaInicial : MonoBehaviour
{
    // Esta função será chamada pelo botão de iniciar
    public void IniciarSimulacao()
    {
        // Salva que o perfil escolhido foi o Jovem
        PlayerPrefs.SetString("PerfilUsuario", "JOVEM");
        PlayerPrefs.Save();

        // Carrega a cena do jogo principal onde as barras funcionam
        SceneManager.LoadScene("Scene_Jogo");
    }
}