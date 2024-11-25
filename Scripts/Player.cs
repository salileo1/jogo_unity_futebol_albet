using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textGoal;
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private Ball ball;
    [SerializeField] private Vector3 startPositionPlayer;

    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Ball ballAttachedToPlayer;
    private float timeShot;
    private float gameTime;
    private const int LAYER_SHOOT = 1;
    private int myScore, otherScore;
    private CharacterController characterController;
    private float distanceSinceLastDribble;
    private float goalTextColorAlpha;

    private AudioSource golSound;
    private AudioSource torcida;

    private float timeLimit;

    private int targetScore;
    private int shootCount;

     private RankingManager rankingManager;

    public Ball BallAttachedToPlayer { get => ballAttachedToPlayer; set => ballAttachedToPlayer = value; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        //definiçoes de dificuldade + contador de chutes
        gameTime = 0f;  
        targetScore = 0;
        shootCount = 0;
        startPositionPlayer = transform.position;
        //ranking
        GameObject rankingManagerObject = new GameObject("RankingManager");
        rankingManager = rankingManagerObject.AddComponent<RankingManager>();

        // Obter o componente de áudio
        torcida = GameObject.Find("Sound/torcida").GetComponent<AudioSource>();
        golSound = GameObject.Find("Sound/sound_gol").GetComponent<AudioSource>();
        torcida.Play();
        

        // Definir o limite de tempo dependendo da cena
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Game")
        {
            timeLimit = 120f;
            targetScore = 5;
        }
        else if (currentScene == "Game2")
        {
            timeLimit = 180f;
            targetScore = 10;
        }
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        int minutes = (int)(gameTime / 60);
        int seconds = (int)(gameTime % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (gameTime >= timeLimit && otherScore < targetScore)
        {   
            string playerName = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            rankingManager.AddRankingEntry(playerName, otherScore);
            GameState.ResultMessage = $"Perdeu! Tente novamente! \nTotal de gols: {otherScore} em {minutes}:{seconds:00} min\n Total de chutes {shootCount}";
            SceneManager.LoadScene("Menu");
            return;
        }
        else if (otherScore >= targetScore)
        {
           string playerName = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            rankingManager.AddRankingEntry(playerName, myScore);
            GameState.ResultMessage = $"Parabéns, você venceu! \nTotal de gols: {otherScore} em {minutes}:{seconds:00} min\n Total de chutes {shootCount}";
            SceneManager.LoadScene("Menu");
        }

        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

        if (starterAssetsInputs.shoot)
        {
            starterAssetsInputs.shoot = false;
            timeShot = Time.time;
            animator.Play("Shoot", LAYER_SHOOT, 0f);
            animator.SetLayerWeight(LAYER_SHOOT, 1f);
            shootCount++;
        }

        if (timeShot > 0)
        {
            // Chutar a bola
            if (ballAttachedToPlayer != null && Time.time - timeShot > 0.2f)
            {
                ballAttachedToPlayer.StickToPlayer = false;
                Rigidbody rigidbody = ballAttachedToPlayer.transform.gameObject.GetComponent<Rigidbody>();
                Vector3 shootdirection = transform.forward;
                shootdirection.y += 0.3f;
                rigidbody.AddForce(shootdirection * 20f, ForceMode.Impulse);
                ballAttachedToPlayer = null;
            }

            if (Time.time - timeShot > 0.5f)
            {
                timeShot = 0;
            }
        }
        else
        {
            animator.SetLayerWeight(LAYER_SHOOT, Mathf.Lerp(animator.GetLayerWeight(LAYER_SHOOT), 0f, Time.deltaTime * 10f));
        }

        if (goalTextColorAlpha > 0)
        {
            goalTextColorAlpha -= Time.deltaTime;
            textGoal.alpha = goalTextColorAlpha;
            textGoal.fontSize = 30;
        }
    }

    public void IncreaseMyScore()
    {
        myScore++;
        golSound.Play();
        UpdateScore();  
    }

    public void IncreaseOtherScore()
    {
        otherScore++;
        golSound.Play();
        UpdateScore();  
    }

    public void ResetarBola()
    {
        if (ball != null)
        {
            ball.recomecar();
        }
    }

    private void UpdateScore()
    {
  

        // Atualizar o placar
        textScore.text = " YOU   " + otherScore + "   " + myScore + "    IA";
        goalTextColorAlpha = 1f;

        // Resetar a posição do jogador
        transform.position = startPositionPlayer;
        transform.rotation = Quaternion.identity;

        ResetarBola();
    }

    
}
