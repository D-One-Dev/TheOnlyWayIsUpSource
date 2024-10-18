using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    [SerializeField] private Spike _spike;
    bool isAwake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAwake)
        {
            _spike.WakeUp();
            isAwake = true;
        }
    }
}
