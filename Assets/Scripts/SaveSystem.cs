using UnityEngine;

public static class SaveSystem
{
    private static string saveKey = "PlayerData"; // Key for PlayerPrefs

    public static void SavePlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        // Create PlayerData using the current player position and health
        PlayerData data = new PlayerData
        {
            position = new float[] { player.transform.position.x, player.transform.position.y, player.transform.position.z },
            health = playerController.currentHealth
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save(); // Save PlayerPrefs
        Debug.Log("Game saved to PlayerPrefs.");
    }

    public static void LoadPlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        // Always reset current health to max health when loading
        playerController.currentHealth = playerController.maxHealth;

        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);

            // Log the health being set to max when loading from saved data
            Debug.Log("Loading player from saved data. Setting health to max health.");

            // Set the player's health to max health regardless of the saved health
            playerController.currentHealth = playerController.maxHealth;

            // Set the save point to the loaded data
            playerController.SetSavePoint(player.transform.position, playerController.currentHealth);

            Debug.Log("Game loaded from PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning("No save data found in PlayerPrefs. Using current player data.");

            // No saved data, just retain current position; health is already set to max above
            player.transform.position = playerController.transform.position;

            // Set the save point to current data since no save file exists
            playerController.SetSavePoint(player.transform.position, playerController.currentHealth);
        }
    }
}
