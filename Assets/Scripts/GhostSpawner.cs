using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghost_prefabs;
    GameObject ghost_instance;

    public float intial_delay_min, initial_delay_max;
    public float respawn_seconds_min, respawn_seconds_max;
    float respawn_seconds;
    public bool timer_active = true;

    float res_timer;
    bool instance_isInactive;
    bool player_special_actived;

    private void OnEnable()
    {
        initialize();
        PlayerController.UndertheLegStateChanged += UpdateVisibleState;
    }

    private void OnDisable()
    {
        PlayerController.UndertheLegStateChanged -= UpdateVisibleState;
    }

    void initialize()
    {
        StartCoroutine(InitialDelay());
    }

    private void Update()
    {
        Timer();
    }

    IEnumerator InitialDelay()
    {
        var initial_delay = Random.Range(intial_delay_min, initial_delay_max);
        yield return new WaitForSeconds(initial_delay);
        Spawn();
    }

    void Timer()
    {
        if (!ghost_instance)
            return;

        instance_isInactive = (!ghost_instance.activeSelf);

        if (instance_isInactive)
        {
            res_timer += Time.deltaTime;
            if (res_timer >= respawn_seconds)
            {
                Spawn();
            }
        }
    }

    public void Spawn()
    {
        if (!ghost_instance)
        {
            ghost_instance = GameObject.Instantiate(ghost_prefabs);
        }

        ghost_instance.transform.position = transform.position;

        var ghost_comp = ghost_instance.GetComponent<Ghost>();

        ghost_instance.SetActive(true);
        ghost_comp.SetVisibility(player_special_actived);
        res_timer = 0;

        respawn_seconds = Random.Range(respawn_seconds_min, respawn_seconds_max);
    }

    void UpdateVisibleState(bool state)
    {
        player_special_actived = state;
    }
}
