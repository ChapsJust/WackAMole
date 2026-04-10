using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum EtatJeu { Menu, EnJeu, GameOver }

    [Header("Canvas")]
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject canvasHUD;
    [SerializeField] private GameObject canvasGameOver;

    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI texteScore;
    [SerializeField] private TextMeshProUGUI texteTimer;
    [SerializeField] private TextMeshProUGUI texteScoreFinal;

    [Header("Références")]
    [SerializeField] private SpawnManager spawnManager;

    [Header("Paramètres")]
    [SerializeField] private float dureePartie = 60f;

    private EtatJeu etatActuel;
    private int score;
    private float tempsRestant;

    void Start()
    {
        ChangerEtat(EtatJeu.Menu);
    }

    void Update()
    {
        if (etatActuel != EtatJeu.EnJeu) return;

        tempsRestant -= Time.deltaTime;
        AfficherTimer();

        if (tempsRestant <= 0f)
        {
            TerminerJeu();
        }
    }

    public void ChangerEtat(EtatJeu nouvelEtat)
    {
        etatActuel = nouvelEtat;
        canvasMenu.SetActive(etatActuel == EtatJeu.Menu);
        canvasHUD.SetActive(etatActuel == EtatJeu.EnJeu);
        canvasGameOver.SetActive(etatActuel == EtatJeu.GameOver);
    }

    public void CommencerJeu()
    {
        score = 0;
        tempsRestant = dureePartie;
        MettreAJourScore();
        AfficherTimer();
        spawnManager.Demarrer();
        ChangerEtat(EtatJeu.EnJeu);
    }

    public void CibleFrappee(Cible cible)
    {
        if (etatActuel != EtatJeu.EnJeu) return;
        score += 10;
        MettreAJourScore();
        cible.Frapper();
    }

    private void TerminerJeu()
    {
        spawnManager.Arreter();
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Cible"))
        {
            Destroy(c);
        }
        texteScoreFinal.text = $"Score final : {score}";
        ChangerEtat(EtatJeu.GameOver);
    }

    public void Rejouer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MettreAJourScore()
    {
        texteScore.text = $"Score : {score}";
    }

    private void AfficherTimer()
    {
        int secondes = Mathf.Max(0, Mathf.CeilToInt(tempsRestant));
        texteTimer.text = $"Temps : {secondes}s";
    }
}