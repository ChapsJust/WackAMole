using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabCible;
    [SerializeField] private GameManager gameManager;

    [Header("Zone de spawn")]
    [SerializeField] private Transform joueur;
    [SerializeField] private float rayonMin = 1.2f;
    [SerializeField] private float rayonMax = 2.0f;
    [SerializeField] private float hauteurRelMin = -0.5f;
    [SerializeField] private float hauteurRelMax = 0.3f;

    [Header("Timing")]
    [SerializeField] private float intervalleSpawn = 1.5f;
    [SerializeField] private float delaiDisparition = 3f;
    [SerializeField] private int maxCiblesActives = 3;

    [SerializeField] private float intervalleSpawnFinal = 0.5f;
    [SerializeField] private float delaiDisparitionFinal = 1.5f;
    [SerializeField] private int maxCiblesActivesFinal = 5;

    private float intervalleActuel;
    private float delaiActuel;
    private int maxActuel;

    private float prochainSpawn;
    private bool actif;
    private List<Cible> ciblesActives = new List<Cible>();

    void Start()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
        if (joueur == null && Camera.main != null)
            joueur = Camera.main.transform;
    }

    public void Demarrer()
    {
        actif = true;
        prochainSpawn = Time.time;
        intervalleActuel = intervalleSpawn;
        delaiActuel = delaiDisparition;
        maxActuel = maxCiblesActives;
    }

    public void MettreAJourDifficulte(float progression)
    {
        intervalleActuel = Mathf.Lerp(intervalleSpawn, intervalleSpawnFinal, progression);
        delaiActuel = Mathf.Lerp(delaiDisparition, delaiDisparitionFinal, progression);
        maxActuel = maxCiblesActives + Mathf.RoundToInt(Mathf.Lerp(maxCiblesActives, maxCiblesActivesFinal, progression));
    }

    public void Arreter()
    {
        actif = false;
    }

    /// <summary>
    /// Vérifie à chaque frame si le spawn est actif, si le délai entre les spawns est écoulé, et si le nombre de cibles actives est inférieur au maximum autorisé.
    /// </summary>
    void Update()
    {
        if (!actif) return;
        if (Time.time < prochainSpawn) return;
        if (ciblesActives.Count >= maxActuel) return;

        SpawnerCible();
        prochainSpawn = Time.time + intervalleActuel;
    }

    /// <summary>
    /// Permet de retirer une cible de la liste des cibles actives.
    /// </summary>
    /// <param name="cible">La cible à retirer.</param>
    public void CibleDetruite(Cible cible)
    {
        ciblesActives.Remove(cible);
    }

    /// <summary>
    /// Permet de faire disparaître toutes les cibles actives, puis vide la liste des cibles actives.
    /// </summary>
    public void DetruireToutesCibles()
    {
        List<Cible> copie = new List<Cible>(ciblesActives);
        foreach (Cible cible in copie)
            cible.Disparaitre();
        ciblesActives.Clear();
    }

    /// <summary>
    /// Génère une position aléatoire autour du joueur dans une zone définie, puis instancie une cible à cette position.
    /// La cible est orientée pour faire face au joueur.
    /// </summary>
    private void SpawnerCible()
    {
        // Calculer une position aléatoire autour du joueur
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float rayon = Random.Range(rayonMin, rayonMax);
        Vector3 centre = joueur != null ? joueur.position : Vector3.zero;
        Vector3 offset = new Vector3(Mathf.Cos(angle) * rayon, Random.Range(hauteurRelMin, hauteurRelMax), Mathf.Sin(angle) * rayon);
        Vector3 position = centre + offset;

        GameObject go = Instantiate(prefabCible, position, Quaternion.identity);
        go.tag = Tags.Cible;

        // Orienter la cible vers le joueur
        Vector3 dirVersJoueur = new Vector3(centre.x - position.x, 0f, centre.z - position.z);
        if (dirVersJoueur != Vector3.zero)
            go.transform.rotation = Quaternion.LookRotation(dirVersJoueur);

        Cible cible = go.GetComponent<Cible>();
        ciblesActives.Add(cible);
        cible.Initialiser(gameManager, this, delaiActuel);
    }
}
