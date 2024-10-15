using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; // Required to access the PlayableDirector
using UnityEngine.SceneManagement; // Required to change scenes

public class GoToNextSceneTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector; // Reference to the PlayableDirector component
    public string nextSceneName; // Name of the next scene to load

    // Start is called before the first frame update
    void Start()
    {
        if (playableDirector == null)
        {
            // Attempt to automatically find the PlayableDirector if not set in the inspector
            playableDirector = GetComponent<PlayableDirector>();
        }
        
        // Ensure we have a valid PlayableDirector reference
        if (playableDirector != null)
        {
            // Subscribe to the event that is triggered when the timeline finishes
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    // This method is called when the timeline stops (i.e., when it finishes)
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // Check if the director that stopped is the one we're monitoring
        if (director == playableDirector)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // Optional: if you need to handle any cleanup when the object is destroyed
    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
