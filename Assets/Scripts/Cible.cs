using UnityEngine;

public class Cible : MonoBehaviour
{
    private GameManager gameManager;
    private float delaiDisparition = 3f;

    public void Initialiser(GameManager gm, float delai)
    {
        gameManager = gm;
        delaiDisparition = delai;
        Invoke(nameof(Disparaitre), delaiDisparition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Marteau"))
        {
            gameManager.CibleFrappee(this);
        }
    }

    public void Disparaitre()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}