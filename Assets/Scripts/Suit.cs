using UnityEngine;

[CreateAssetMenu(fileName = "Suit", menuName = "Suit")]
public class Suit : ScriptableObject
{
    public Color color;
    public int health;
    public float movementSpeed;
    public int cost;
    public string startGun;
}
