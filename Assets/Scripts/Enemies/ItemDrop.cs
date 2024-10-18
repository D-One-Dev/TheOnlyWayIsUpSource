using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] items; //available items 
    [SerializeField] private int goldAmount; //amount of gold dropped on death
    [SerializeField] private GameObject goldPrefab; //gold prefab
    [SerializeField] private float dropForce; //items drop force

    public void DropItem() //dropping item
    {
        int itemIndex = Random.Range(0, items.Length); //choosing random item
        if (items[itemIndex] != null) //if there is an item
        {
            Vector2 vect = new Vector2(Random.Range(-1f, 1f), 1f); //choosing random vector
            GameObject item = Instantiate(items[itemIndex], transform.position, Quaternion.identity); //instantiating item
            item.GetComponent<Rigidbody2D>().AddForce(vect * dropForce); //adding force to item
        }

        for(int i = 0; i < goldAmount; i++) //dropping gold
        {
            Vector2 vect = new Vector2(Random.Range(-1f, 1f), 1f); //choosing random vector
            GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity); //instantiating gold
            gold.GetComponent<Rigidbody2D>().AddForce(vect * dropForce); //adding force to gold
        }
    }
}
