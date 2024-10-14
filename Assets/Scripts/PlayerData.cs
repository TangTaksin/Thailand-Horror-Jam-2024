using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position; // Array to hold x, y, z positions
    public float health;

    // Parameterless constructor for JsonUtility
    public PlayerData() 
    {
        position = new float[3]; // Initialize array to avoid null reference
    }

    // Constructor for easy creation
    public PlayerData(Vector3 position, float health)
    {
        this.position = new float[] { position.x, position.y, position.z };
        this.health = health;
    }
}
