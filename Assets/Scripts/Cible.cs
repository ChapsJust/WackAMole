using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Cible : MonoBehaviour
{
    private GameManager gameManager;
    private float delaiDisparition = 3f;
    private Coroutine disparitionCoroutine;

    public void Initialiser(GameManager gm, float delai)
    {
        gameManager = gm;
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
        if (rb != null && other.CompareTag("Marteau"))
        {
            gameManager.CibleFrappee(this);
        }
    }

    public void Frapper()
    {
        if (disparitionCoroutine != null)
        {
            StopCoroutine(disparitionCoroutine);
        }
        GetComponent<Collider>().enabled = false;
        StartCoroutine(EffetImpact());
    }

    //Aide de ChatGPT pour faire un effet animation
    private IEnumerator EffetImpact()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 echelle = transform.localScale;

        rend.material.color = Color.yellow;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.7f;
            transform.localScale = Vector3.Lerp(echelle, echelle * 1.5f, t);
            yield return null;
        }

        Disparaitre();
    }

    public void Disparaitre()
    {
        if (disparitionCoroutine != null)
        {
            StopCoroutine(disparitionCoroutine);
        }
        Destroy(gameObject);
    }
}