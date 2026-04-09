using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabCible;
    [SerializeField] private GameManager gameManager;

    [Header("Zone de spawn")]
    [SerializeField] private Vector3 zoneMin = new Vector3(-1f, 0.8f, -0.5f);
    [SerializeField] private Vector3 zoneMax = new Vector3(1f, 1.8f, 0.5f);

    [Header("Timing")]
    [SerializeField] private float intervalleSpawn = 1.5f;
    [SerializeField] private float delaiDisparition = 3f;
    [SerializeField] private int maxCiblesActives = 3;

    private float prochainSpawn;
    private bool actif;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
    }

    public void Demarrer()
    {
        actif = true;
        prochainSpawn = Time.time;
    }

    public void Arreter()
    {
        actif = false;
    }

    void Update()
    {
        if (!actif) return;
        if (Time.time < prochainSpawn) return;

        int ciblesActuelles = GameObject.FindGameObjectsWithTag("Cible").Length;
        if (ciblesActuelles >= maxCiblesActives) return;

        SpawnerCible();
        prochainSpawn = Time.time + intervalleSpawn;
    }

    private void SpawnerCible()
    {
        Vector3 position = new Vector3(
            Random.Range(zoneMin.x, zoneMax.x),
            Random.Range(zoneMin.y, zoneMax.y),
            Random.Range(zoneMin.z, zoneMax.z)
        );

        GameObject go = Instantiate(prefabCible, position, Quaternion.identity);
        go.tag = "Cible";
        go.GetComponent<Cible>().Initialiser(gameManager, delaiDisparition);
    }
}
