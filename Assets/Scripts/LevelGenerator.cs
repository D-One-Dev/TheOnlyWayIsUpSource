using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] Rooms; //pool of rooms
    [SerializeField] private GameObject grid; //tileset grid
    [SerializeField] private GameObject startRoom; //start room
    [SerializeField] private GameObject endRoom; //end room
    [SerializeField] private GameObject bossRoom; //boss room
    [SerializeField] private int numberOfRooms; //number of rooms in current level
    [SerializeField] private float roomHeight = 10.5f; //height of a room (default=10.5f)
    [SerializeField] private bool isBossLevel;
    private GameObject currRoom; //current room
    void Start()
    {
        float startTime = Time.realtimeSinceStartup; //counting time for debugging
        currRoom = startRoom; //setting the start room
        for (int i = 0; i < numberOfRooms - 1; i++)
        {//calculating new room position
            Vector3 newPos = CalculateNewRoomPos();
            int newRoomID = Random.Range(0, Rooms.Length); //choosing new room
            currRoom = Instantiate(Rooms[newRoomID], newPos, Quaternion.identity, grid.transform); //instantiating new room
        }

        if (isBossLevel) //spawning boss room
        {
            Vector3 newPos = CalculateNewRoomPos();
            currRoom = Instantiate(bossRoom, newPos, Quaternion.identity, grid.transform);
        }

        //instantiating end room
        Vector3 pos = CalculateNewRoomPos();
        currRoom = Instantiate(endRoom, pos, Quaternion.identity, grid.transform);

        AstarPath.active.Scan(); //updating a* grid

        Debug.LogFormat("Level built in " + (Time.realtimeSinceStartup - startTime).ToString() + " seconds."); //logging time for debugging
    }

    private Vector3 CalculateNewRoomPos()
    {
        return new Vector3(currRoom.transform.position.x, currRoom.transform.position.y + roomHeight, currRoom.transform.position.z);
    }
}
