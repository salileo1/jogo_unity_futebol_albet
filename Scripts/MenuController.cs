using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText; // Referência ao componente de texto na UI
    [SerializeField] private TextMeshProUGUI rankingText;

    [SerializeField] private RankingManager rankingManager;

    private void Start()
    {
        rankingManager = FindObjectOfType<RankingManager>();

        if (rankingManager != null)
        {
            rankingManager.LoadRanking(); // Garante que os dados mais recentes sejam carregados
        }

        if (string.IsNullOrEmpty(GameState.ResultMessage))
        {
            GameState.ResultMessage = "Selecione o modo de jogo";
        }

        // Exibe a mensagem atual no resultText
        if (resultText != null)
        {
            resultText.text = GameState.ResultMessage;
        }

        // Exibe o ranking no rankingText
        DisplayRanking();
    }

    private void DisplayRanking()
    {
        if (rankingManager != null && rankingManager.ranking.Count > 0)
        {
            string rankingDisplay = "Ranking:\n";
            foreach (var entry in rankingManager.ranking)
            {
                rankingDisplay += $"{entry.playerName}: {entry.totalGoals} gols\n";
            }
            rankingText.text = rankingDisplay;
        }
        else
        {
            rankingText.text = "Nenhum dado de ranking disponível.";
        }
    }

    public void StartGameFacil()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartGameDificil()
    {
        SceneManager.LoadScene("Game2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
