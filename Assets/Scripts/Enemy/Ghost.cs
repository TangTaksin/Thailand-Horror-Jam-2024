using UnityEngine;
using UnityEngine.Events;

public class Ghost : MonoBehaviour
{
    SpriteRenderer ghostRenderer;
    protected Rigidbody2D rigid2d;
    protected Transform player;

    protected bool _isInactive;
    protected bool _IsVisible;
    protected bool _playerHide;
    protected float ghostDir;

    public float detectionRange = 5f;
    bool playerDetected;
    protected bool isDetectingPlayer 
    {
        get { return playerDetected; }
        set 
        {
            if (playerDetected == value)
                return;

            playerDetected = value;
            print("player detected = " + playerDetected);
            OnPlayerDetected(playerDetected);
        }
    }

    bool _isSeen;
    public bool isSeen
    {
        get { return _isSeen; }
        set
        {
            _isSeen = value;
            OnSeen();
        }
    }

    public bool seenDecayActive; 
    public float isSeenDecay;
    float decayTimer;

    public UnityEvent OnSeenEvent, OnUnseenEvent;

    protected virtual void OnEnable()
    {
        ghostRenderer = GetComponent<SpriteRenderer>();
        rigid2d = GetComponent<Rigidbody2D>();

        PlayerController.UndertheLegStateChanged += OnPlayerSpecial;
        PlayerController.HideStateChanged += OnPlayerHide;

        Initialize();
    }

    private void OnDisable()
    {
        PlayerController.UndertheLegStateChanged -= OnPlayerSpecial;
        PlayerController.HideStateChanged -= OnPlayerHide;
    }

    protected virtual void Initialize()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        isSeen = false;
    }

    protected virtual void Update()
    {
        BeingSeenProcess();
        CheckForPlayerDetection();
        SpriteUpdate();
    }

    protected virtual void SpriteUpdate()
    {
        var normaVel = rigid2d.velocity.normalized;
        var xDir = normaVel.x;
        ghostDir = Mathf.Lerp(ghostDir, xDir, Mathf.Abs(xDir));
        ghostRenderer.flipX = (ghostDir > 0);
    }

    public void SetBeingSeen()
    {
        isSeen = true;
    }

    protected virtual void BeingSeenProcess()
    {
        

        if (isSeen && seenDecayActive)
        {
            print("being seen progress is running");

            decayTimer -= Time.deltaTime;

            if (decayTimer <= 0)
            {
                isSeen = false;
            }
        }
        
        SetVisibility();
    }

    protected virtual void OnSeen()
    {
        if (isSeen)
        {
            print("Seen by player");
            decayTimer = isSeenDecay;
            OnSeenEvent?.Invoke();
        }
        else
        {
            OnUnseenEvent?.Invoke();
        }
    }

    protected void CheckForPlayerDetection()
    {
        isDetectingPlayer = (isSeen && Vector3.Distance(transform.position, player.position) <= detectionRange);
    }

    protected virtual void OnPlayerSpecial(bool value)
    {

    }

    public void SetVisibility()
    {
        var eval = decayTimer / isSeenDecay;
        ghostRenderer.color = new Color(1, 1, 1, eval);
    }

    protected void OnPlayerHide(bool value)
    {
        _playerHide = value;
    }

    protected virtual void OnPlayerDetected(bool value)
    {

    }
}
