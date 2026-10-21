using UnityEngine;
using Fusion;
using TMPro;

public class ShootHUD : MonoBehaviour
{
    [SerializeField] private NetworkRunner runner;

    [Header("HUD")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text matchStatus;

    [Header("Resultado")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TMP_Text resultText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resultsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (runner == null || !runner.IsRunning) return;

        ShootMatch match = ShootMatch.Instance;

        if(match == null)
        {
            matchStatus.text = "Aguardando estado da partida...";
            return;
        }

        UpdateHealth();

        if (match.IsOver)
        {
            ShowResult(match);
        }
        else
        {
            resultsPanel.SetActive(false);

            matchStatus.text = match.CanPlay ? "Jogo em andamento" : "Aguardando Segundo Jogador...";
        }
    }

    private void UpdateHealth()
    {
        if(runner.TryGetPlayerObject(runner.LocalPlayer, out NetworkObject playerObject))
        {
            ShootHealth health = playerObject.GetComponent<ShootHealth>();

            if(health != null)
            {
                healthText.text = "Vida: " + health.health + " / 100";
            }
                
        }
    }

    private void ShowResult(ShootMatch match)
    {
        resultsPanel.SetActive(true);

        matchStatus.text = "Partida encerrada";

        bool localPlayerWon = match.Winner == runner.localPlayer;

        string winnerName = match.Winner = match.BluePlayer ? "Azul" : "Vermelho";

        string message = localPlayerWon ? "Você venceu" : "Você perdeu";

        resultText.text = message + "\n Vencedor: Jogador " + winnerName;
    }
}
