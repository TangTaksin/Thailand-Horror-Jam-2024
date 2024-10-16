using UnityEngine;

public class Ghost : MonoBehaviour
{
    SpriteRenderer ghostRenderer;
    protected Rigidbody2D rigid2d;
    protected Transform player;

    protected bool _isVanish;
    protected bool _IsVisible;
    protected bool _playerHide;

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
            if (isSeen)
                OnSeen();
        }
    }
    public float isSeenDecay;
    float decayTimer;

    private void OnEnable()
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

        SetVisibility(false);
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
        ghostRenderer.flipX = (xDir < 0);
    }

    public void SetBeingSeen()
    {
        decayTimer = isSeenDecay;
        isSeen = true;
    }

    protected virtual void BeingSeenProcess()
    {

    }

    protected virtual void OnSeen()
    {

    }

    protected void CheckForPlayerDetection()
    {
        isDetectingPlayer = (isSeen && Vector3.Distance(transform.position, player.position) <= detectionRange);
    }

    protected virtual void OnPlayerSpecial(bool value)
    {
        SetVisibility(value);
    }

    void SetVisibility(bool value)
    {
        _IsVisible = value;
        ghostRenderer.enabled = _IsVisible;
    }

    protected void OnPlayerHide(bool value)
    {
        _playerHide = value;
    }

    protected virtual void OnPlayerDetected(bool value)
    {

    }
}
