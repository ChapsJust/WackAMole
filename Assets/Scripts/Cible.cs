using System.Collections;
using UnityEngine;

public class Cible : MonoBehaviour
{
    private GameManager gameManager;
    private SpawnManager spawnManager;
    private float delaiDisparition = 3f;
    private Coroutine disparitionCoroutine;
    private bool detruite;

    public void Initialiser(GameManager gm, SpawnManager sm, float delai)
    {
        gameManager = gm;
        spawnManager = sm;
        delaiDisparition = delai;
        disparitionCoroutine = StartCoroutine(AttendrePuisDisparaitre());
    }

    private IEnumerator AttendrePuisDisparaitre()
    {
        yield return new WaitForSeconds(delaiDisparition);
        Disparaitre();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager == null) return;
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && other.CompareTag(Tags.Marteau))
        {
            gameManager.CibleFrappee(this);
        }
    }

    /// <summary>
    /// Fonction appelée par le GameManager lorsque la cible est frappée. Arrête la coroutine de disparition,
    /// désactive le collider pour éviter les collisions supplémentaires, puis lance une coroutine d'effet d'impact avant de faire disparaître la cible.
    /// </summary>
    public void Frapper()
    {
        if (disparitionCoroutine != null)
        {
            StopCoroutine(disparitionCoroutine);
        }
        GetComponent<Collider>().enabled = false;
        StartCoroutine(EffetImpact());
    }

    /// <summary>
    /// Affiche un effet visuel d'impact (changement de couleur et agrandissement temporaire), puis fait disparaître la cible.
    /// Aide ChatGPT
    /// </summary>
    /// <returns></returns>
    private IEnumerator EffetImpact()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 echelle = transform.localScale;

        rend.material.color = Color.yellow;

        float t = 0f;
        while (t < 1f)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / 0.04f);
            transform.localScale = Vector3.Lerp(echelle, echelle * 1.5f, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / 0.05f);
            transform.localScale = Vector3.Lerp(echelle * 1.5f, Vector3.zero, t);
            yield return null;
        }

        Disparaitre();
    }

    /// <summary>
    /// Arrête la coroutine de disparition si elle est en cours, puis détruit l'objet cible.
    /// </summary>
    public void Disparaitre()
    {
        if (detruite) return;
        detruite = true;
        if (disparitionCoroutine != null)
        {
            StopCoroutine(disparitionCoroutine);
            disparitionCoroutine = null;
        }
        spawnManager?.CibleDetruite(this);
        Destroy(gameObject);
    }
}