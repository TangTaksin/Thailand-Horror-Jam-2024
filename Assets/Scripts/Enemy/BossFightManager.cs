using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFightManager : MonoBehaviour
{
    public List<GhostSpawner> ghostSpawners;
    public List<BossPart> bossParts;

    private int bossProgress;
    private int bossProgressGoal;

    private void OnEnable()
    {
        BossPart.OnPartBreak += HandlePartBreak;
        PlayerController.OnDeath += Initialize;
        Initialize();
    }

    private void OnDisable()
    {
        BossPart.OnPartBreak -= HandlePartBreak;
        PlayerController.OnDeath -= Initialize;
    }

    public void Initialize()
    {
        bossProgress = 0;
        bossProgressGoal = bossParts.Count;

        // Deactivate all ghost spawners and boss parts initially
        ghostSpawners.ForEach(gs => gs.gameObject.SetActive(false));
        bossParts.ForEach(bp => bp.gameObject.SetActive(false));

        // Activate the first boss part
        if (bossParts.Count > 0)
        {
            bossParts[0].gameObject.SetActive(true);
        }
    }

    private void HandlePartBreak(GameObject brokenPart)
    {
        bossProgress++;
        int partIndex = bossParts.IndexOf(brokenPart.GetComponent<BossPart>());

        // Activate corresponding ghost spawner
        if (partIndex >= 0 && partIndex < ghostSpawners.Count)
        {
            ghostSpawners[partIndex].gameObject.SetActive(true);
        }

        // Activate the next boss part, if available
        if (partIndex + 1 < bossParts.Count)
        {
            bossParts[partIndex + 1].gameObject.SetActive(true);
        }

        CheckBossProgress();
    }

    private void CheckBossProgress()
    {

        if (bossProgress >= bossProgressGoal)
        {
            AudioManager.Instance.PlaySFXClone(AudioManager.Instance.damageSfx);
            AudioManager.Instance.ChangeMusic(AudioManager.Instance.musicBg);
            SceneManager.LoadScene("CutSceneEnd");  // Load ending scene when boss is defeated
        }
    }
}
