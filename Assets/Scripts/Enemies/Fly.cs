using Pathfinding;
using UnityEngine;

public class Fly : MonoBehaviour
{
    
    [SerializeField] private float idleMovement; // idle movement amount
    [SerializeField] private GameObject parent;
    private bool isAwake; // is enemy awake
    public void WakeUp(GameObject player) //waking up on player approach
    {
        isAwake = true;
        parent.GetComponent<AIDestinationSetter>().target = player.transform; //setting player as target
    }

    private void FixedUpdate() // idle movement
    {
        if (!isAwake)
        {
            Vector2 bias = new Vector2(0f, Mathf.Sin(Time.time) * idleMovement);
            parent.transform.position += (Vector3)bias;
        }
    }
}
