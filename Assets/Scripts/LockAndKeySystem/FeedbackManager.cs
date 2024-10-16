using UnityEngine;
using TMPro; // Include this for TextMeshPro components
using System.Collections; // Needed for coroutines

public class FeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI feedbackText; // Reference to the TextMeshProUGUI component
    public GameObject feedbackBackground; // Reference to the background image GameObject
    private Coroutine feedbackCoroutine; // Store the currently running coroutine

    private void Start()
    {
        // Hide the feedback text and background at the start
        feedbackText.gameObject.SetActive(false);
        feedbackBackground.SetActive(false); // Hide background image
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message; // Set the feedback text
        feedbackText.gameObject.SetActive(true); // Show the text
        feedbackBackground.SetActive(true); // Show the background image

        // If there is an active coroutine, stop it
        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
        }

        // Start a new coroutine to hide the feedback
        feedbackCoroutine = StartCoroutine(HideFeedback(3.5f)); // You can pass in the desired delay here
    }

    public void HideFeedback()
    {
        feedbackText.gameObject.SetActive(false); // Hide the text
        feedbackBackground.SetActive(false); // Hide the background
    }

    private IEnumerator HideFeedback(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideFeedback(); // Call the hide method after the delay
    }
}
