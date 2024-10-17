using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour, IInteractable
{
    public Vector3 position { get { return transform.position; } }

    [SerializeField] bool _isInteractable;
    [SerializeField] string sceneToLoad;  // Scene to load normally
    [SerializeField] string sceneToLoadAdditively;  // Scene to load additively

    public bool isInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }

    public void Interact(object _interacter)
    {
        if (isInteractable)
        {
            LoadScene();  // Regular scene load
            //LoadSceneOver();
        }
    }

    // This method loads the scene normally (replacing the current one)
    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);  // Load scene by name
        }
        else
        {
            Debug.LogWarning("Scene name is not set for the SceneChanger.");
        }
    }

    // This method loads the scene additively (over the current one)
    public void LoadSceneOver()
    {
        if (!string.IsNullOrEmpty(sceneToLoadAdditively))
        {
            SceneManager.LoadScene(sceneToLoadAdditively, LoadSceneMode.Additive);  // Load scene additively
        }
        else
        {
            Debug.LogWarning("Additive scene name is not set for the SceneChanger.");
        }
    }
}
