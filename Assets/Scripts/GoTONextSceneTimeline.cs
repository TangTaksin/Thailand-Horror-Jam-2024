using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; // Required to access the PlayableDirector
using UnityEngine.SceneManagement; // Required to change scenes
using UnityEngine.UI; // Required for UI components

public class GoToNextSceneTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector; // Reference to the PlayableDirector component
    public string nextSceneName; // Name of the next scene to load
    public Image holdProgressBar; // Reference to the UI Image for progress bar

    private float spacebarHoldTime = 0f; // Tracks how long the spacebar is held
    private const float requiredHoldTime = 2f; // Time required to trigger the action

    // Start is called before the first frame update
    void Start()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }

        // Ensure the progress bar starts empty
        if (holdProgressBar != null)
        {
            holdProgressBar.fillAmount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the spacebar is being held down
        if (Input.GetKey(KeyCode.Space))
        {
            // Increment the hold time
            spacebarHoldTime += Time.deltaTime;

            // Update the progress bar fill amount
            if (holdProgressBar != null)
            {
                holdProgressBar.fillAmount = Mathf.Clamp01(spacebarHoldTime / requiredHoldTime);
            }

            // If the spacebar has been held for the required time, stop the timeline
            if (spacebarHoldTime >= requiredHoldTime)
            {
                if (playableDirector != null && playableDirector.state == PlayState.Playing)
                {
                    playableDirector.Stop();
                    OnPlayableDirectorStopped(playableDirector);
                }

                // Reset the hold time
                spacebarHoldTime = 0f;

                // Clear the progress bar
                if (holdProgressBar != null)
                {
                    holdProgressBar.fillAmount = 0f;
                }
            }
        }
        else
        {
            // Reset the hold time and progress bar if the key is released
            spacebarHoldTime = 0f;

            if (holdProgressBar != null)
            {
                holdProgressBar.fillAmount = 0f;
            }
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            if (nextSceneName == "BossRoom")
            {
                AudioManager.Instance.ChangeMusic(AudioManager.Instance.bossRoomBg);
            }
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
