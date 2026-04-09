using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class FeedbackMarteau : MonoBehaviour
{
    [Header("Haptique - Grab")]
    [SerializeField] private float amplitudeGrab = 0.3f;
    [SerializeField] private float dureeGrab = 0.1f;

    [Header("Haptique - Frappe")]
    [SerializeField] private float amplitudeFrappe = 0.8f;
    [SerializeField] private float dureeFrappe = 0.2f;

    [Header("Audio - Frappe")]
    [SerializeField] private AudioClip sonFrappe;

    private XRGrabInteractable grabInteractable;
    private XRBaseInputInteractor interacteurActif;
    private AudioSource audioSource;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interacteurActif = args.interactorObject as XRBaseInputInteractor;
        interacteurActif?.SendHapticImpulse(amplitudeGrab, dureeGrab);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        interacteurActif = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cible"))
        {
            interacteurActif?.SendHapticImpulse(amplitudeFrappe, dureeFrappe);

            if (audioSource != null && sonFrappe != null)
                audioSource.PlayOneShot(sonFrappe);
        }
    }
}