using System.Collections;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private float lifetime; //gold lifetime
    [SerializeField] private Animator animator;
    void Start()
    {
        StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime() //lifetime
    {
        yield return new WaitForSeconds(lifetime/2);
        animator.SetTrigger("StartFlash"); //play flashing animation
        yield return new WaitForSeconds(lifetime/2);
        Destroy(gameObject);
    }
}