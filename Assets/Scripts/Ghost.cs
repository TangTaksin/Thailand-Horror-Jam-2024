using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;       
    public float moveSpeed = 2f;           
    private int currentPointIndex = 0;     

    [Header("Detection Settings")]
    public float detectionRadius = 5f;     
    private Transform player;               
    private bool isChasing = false;         
    private bool isDetectingPlayer = false; 

    [Header("Chase Settings")]
    public float minChaseDelay = 1f;       
    public float maxChaseDelay = 3f;       
    private float chaseDelay;               

    [Header("Visibility Settings")]
    public bool isVisible = false;          
    public Renderer enemyRenderer;          

    private bool isHiding = false;          
    private float stopDuration = 3f;        
    private float stopTime = 0f;            
    private Vector3 hidingSpot;             

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyRenderer.enabled = isVisible;  
    }

    private void Update()
    {
        if (player == null) return; 

        if (!isChasing)
        {
            Patrol();                   
            CheckForPlayerDetection();  
        }
        else
        {
            HandleChasing();           
        }
    }

    private void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; 
        }
    }

    private void CheckForPlayerDetection()
    {
        if (isVisible && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            if (!isDetectingPlayer)
            {
                isDetectingPlayer = true;
                SetRandomChaseDelay();
                StartCoroutine(ChaseAfterDelay());
            }
        }
    }

    private void SetRandomChaseDelay()
    {
        chaseDelay = Random.Range(minChaseDelay, maxChaseDelay);
        Debug.Log($"Random chase delay set to: {chaseDelay}");
    }

    private IEnumerator ChaseAfterDelay()
    {
        Debug.Log("Ghost has detected the player. Waiting to start chase...");
        yield return new WaitForSeconds(chaseDelay);

        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasing = true; 
            Debug.Log("Ghost has started chasing the player.");
        }

        isDetectingPlayer = false;
    }

    private void HandleChasing()
    {
        if (!isVisible)
        {
            isChasing = false; 
            Debug.Log("Ghost stopped chasing because it is invisible.");
            return; 
        }

        if (isHiding)
        {
            StopAtHidingSpot(); 
        }
        else
        {
            ChasePlayer(); 
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        var playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.isHiding)
        {
            isHiding = true;             
            hidingSpot = player.position; 
        }

        if (Vector3.Distance(transform.position, player.position) > detectionRadius * 1.5f)
        {
            isChasing = false; 
            Debug.Log("Ghost stopped chasing the player because they are out of range.");
        }
    }

    private void StopAtHidingSpot()
    {
        transform.position = Vector3.MoveTowards(transform.position, hidingSpot, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, hidingSpot) < 0.2f)
        {
            stopTime += Time.deltaTime; 
            if (stopTime >= stopDuration)
            {
                ResetChaseState(); 
            }
        }
    }

    private void ResetChaseState()
    {
        isVisible = false; 
        isHiding = false; 
        stopTime = 0f;    
        isChasing = false; 
        enemyRenderer.enabled = isVisible; 
        Debug.Log("Ghost reset chase state after stopping at the hiding spot.");
    }

    public void ToggleVisibility(bool underLegsMode)
    {
        isVisible = underLegsMode; 
        enemyRenderer.enabled = isVisible; 
        Debug.Log($"Ghost visibility toggled: {isVisible}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
