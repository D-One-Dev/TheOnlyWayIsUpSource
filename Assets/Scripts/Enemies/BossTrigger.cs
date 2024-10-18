using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private string bossType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (bossType)
            {
                case "boss1":
                    boss.GetComponentInChildren<Boss1>().WakeUp();
                    Debug.Log("Boss wakes up");
                    break;
                default:
                    Debug.LogWarning("Boss with type" + bossType + "not found!");
                    break;
            }
        }
    }
}
