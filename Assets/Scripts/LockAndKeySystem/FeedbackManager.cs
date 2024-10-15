using UnityEngine;
using TMPro; // Include this for TextMeshPro components
using System.Collections; // Needed for coroutines

public class FeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI feedbackText; // Reference to the TextMeshProUGUI component
    public GameObject feedbackBackground; // Reference to the background image GameObject
    [SerializeField] private float feebBackDelay = 2.0f;

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

        StartCoroutine(HideFeedback(feebBackDelay));
    }

    private IEnumerator HideFeedback(float delay)
    {
        yield return new WaitForSeconds(delay);
        feedbackText.gameObject.SetActive(false); // Hide the text
        feedbackBackground.SetActive(false); // Hide the background
    }
}
