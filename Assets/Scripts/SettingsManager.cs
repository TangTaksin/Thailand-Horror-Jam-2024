using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Daggoot
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("===================Settings=================")]
        [SerializeField] private AudioMixer myMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider ambientSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private GameObject settingPanel;

        private Animator _animator;
        private bool isPanelOpen = false;

        private void Awake()
        {
            _animator = settingPanel.GetComponent<Animator>();
            _animator.enabled = false;

            // Load saved volume preferences
            LoadVolume();

            // Initialize volumes
            SetMusicVolume();
            SetAmbientVolume();
            SetSFXVolume();

            // Add listeners to update volume as sliders change
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            ambientSlider.onValueChanged.AddListener(delegate { SetAmbientVolume(); });
            sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        }

        private void Update()
        {
            // Toggle settings panel when 'P' is pressed
            if (Input.GetKeyDown(KeyCode.P))
            {
                ToggleSettingsPanel();
            }
        }

        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        public void SetAmbientVolume()
        {
            float volume = ambientSlider.value;
            myMixer.SetFloat("Ambient", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("ambientVolume", volume);
        }

        public void SetSFXVolume()
        {
            float volume = sfxSlider.value;
            myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("sfxVolume", volume);
        }

        public void LoadVolume()
        {
            Debug.Log("Loading volume");
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);  // Default volume 0.5
            ambientSlider.value = PlayerPrefs.GetFloat("ambientVolume", 0.5f);
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ToggleSettingsPanel()
        {
            if (isPanelOpen)
            {
                CloseSettingsPanel();
            }
            else
            {
                OpenSettingsPanel();
            }
        }

        public void OpenSettingsPanel()
        {
            settingPanel.SetActive(true);
            SetInteractable(true); // Enable interactions
            _animator.enabled = true;
            _animator.Play("Open_setting_UI_anim");
            isPanelOpen = true;
        }

        public void CloseSettingsPanel()
        {
            SetInteractable(false); // Disable interactions while closing
            _animator.Play("Close_Setting_ui_anim");
            StartCoroutine(DeactivatePanelAfterAnimation());
        }

        private void SetInteractable(bool state)
        {
            musicSlider.interactable = state;
            ambientSlider.interactable = state;
            sfxSlider.interactable = state;
            // Add other UI elements as needed
        }

        private IEnumerator DeactivatePanelAfterAnimation()
        {
            yield return new WaitForSecondsRealtime(_animator.GetCurrentAnimatorStateInfo(0).length);
            settingPanel.SetActive(false);
            _animator.enabled = false;
            isPanelOpen = false;
        }

        public void Resume()
        {
            CloseSettingsPanel();
            // Optionally restore gameplay controls or reset time scale here if needed
            // Example: Time.timeScale = 1f;
        }
    }
}
