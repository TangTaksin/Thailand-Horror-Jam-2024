using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public List<GhostSpawner> ghostSpawners;
    public List<BossPart> bossParts;

    int bossProgress;
    int bossProgressGoal;

    private void OnEnable()
    {
        BossPart.OnPartBreak += OnParkBreak;
        Initialize();
    }

    private void OnDisable()
    {
        BossPart.OnPartBreak -= OnParkBreak;
    }

    public void Initialize()
    {
        bossProgress = 0;
        bossProgressGoal = bossParts.Count;

        foreach (var gs in ghostSpawners)
        {
            gs.gameObject.SetActive(false);
        }

        foreach (var bs in bossParts)
        {
            bs.gameObject.SetActive(true);
        }
    }

    void OnParkBreak(GameObject obj)
    {
        bossProgress++;
        var part = obj.GetComponent<BossPart>();
        var index = bossParts.IndexOf(part);
        ghostSpawners[index].gameObject.SetActive(true);

        BossProgressCheck();
    }

    void BossProgressCheck()
    {
        if (bossProgress >= bossProgressGoal)
        {
            // Boss Clear
        }
    }
}
