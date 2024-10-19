using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public delegate void OnObjectChangeDelegate(object newVal);
    public static OnObjectChangeDelegate OnInteractableChange;

    public delegate void BoolEvent(bool value);
    public static BoolEvent HideStateChanged;
    public static BoolEvent UndertheLegStateChanged;

    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float currentHealth;

    Animator _animator;

    public LayerMask groundLayer;
    bool isGround;
    public float groundCastLenght;
    public float jumpforce;

    private Rigidbody2D rb;
    private float _inputAxis;
    float _facingAxis = 1;
    private Vector3 savePoint;
    private float saveHealth;

    public bool _canAct;

    IInteractable _selectedInteractable;
    public IInteractable selectedInteractable
    {
        get { return _selectedInteractable; }
        set
        {
            if (_selectedInteractable == value) return;

            _selectedInteractable = value;

            if (OnInteractableChange != null)
                OnInteractableChange?.Invoke(_selectedInteractable);
        }
    }

    List<IInteractable> _InteractableList = new List<IInteractable>();

    bool _isHiding = false;
    public bool isHiding
    {
        set
        {
            _isHiding = value;
            HideStateChanged?.Invoke(_isHiding);
        }
        get
        {
            return _isHiding;
        }
    }

    private HidingSpot currentHidingSpot; // Reference to the current hiding spot
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private int normalSortingOrder; // Variable to store the normal sorting order

    public FeedbackManager feedbackManager;

    [Header("Under the Leg Setting")]

    bool underLeg = false; // Tracks whether Under the Legs mode is active
    public bool isUnderLegsMode
    {
        get { return underLeg; }
        set
        {
            if (underLeg == value)
                return;

            underLeg = value;
            UndertheLegStateChanged?.Invoke(underLeg);
        }
    }

    public float DetectionRadius = 1f;
    public Vector2 DetectionOffset;

    public GameObject SpecialOverlay;
    public GameObject SpecialMask;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        normalSortingOrder = spriteRenderer.sortingOrder; // Store the initial sorting order
        InitializePlayer();
    }

    void Update()
    {
        HandleMovement();
        CheckGround();
        SimulateDamage();
        UnderLegProcess();

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

        DecideSelectedInteractable();

        MovementAnimation();
    }

    private void InitializePlayer()
    {
        currentHealth = maxHealth; // Set full health at the start
        savePoint = transform.position; // Initialize save point to the starting position
        saveHealth = maxHealth; // Initialize save health to max health
    }

    #region Movement

    public void OnMove(InputValue value)
    {
        _inputAxis = value.Get<float>();
        _facingAxis = Mathf.Lerp(_facingAxis, _inputAxis, Mathf.Abs(_inputAxis));
    }

    private void HandleMovement()
    {
        if (isHiding || isUnderLegsMode || !_canAct) // Prevent movement while hiding
        {
            rb.velocity = Vector2.zero; // Stop player movement when hiding
            return;
        }

        rb.velocity = new Vector2(_inputAxis * moveSpeed, rb.velocity.y);
        // Play walking sound if the player is moving
        if (Mathf.Abs(_inputAxis) > 0.1f) // Check if there is significant input
        {
            AudioManager.Instance.PlayWalkingSFX();
            
        }
    }

    void CheckGround()
    {
        var groundcast = Physics2D.Raycast(transform.position, Vector2.down, groundCastLenght, groundLayer);
        isGround = (groundcast);

    }

    public void OnJump()
    {
        if (isGround)
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
    }

    #endregion

    #region Interact

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interact = collision.GetComponent<IInteractable>();


        if (interact is IInteractable)
        {
            _InteractableList.Add(interact);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var interact = collision.GetComponent<IInteractable>();

        if (_InteractableList.Contains(interact))
        {
            if (_selectedInteractable == interact)
                selectedInteractable = null;

            _InteractableList.Remove(interact);

        }

        if (_InteractableList.Count == 0)
        {
            selectedInteractable = null;
        }
    }

    void DecideSelectedInteractable()
    {
        if (_InteractableList.Count == 0)
            return;

        float ClosestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach (var interact in _InteractableList)
        {
            if (!interact.isInteractable)
                continue;

            Vector3 DifferenceToTarget = interact.position - currentPosition;
            float DistanceToTarget = DifferenceToTarget.sqrMagnitude;

            if (DistanceToTarget < ClosestDistance)
            {
                ClosestDistance = DistanceToTarget;
                selectedInteractable = interact;
            }
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!_canAct || _isHiding || isUnderLegsMode)
           return;

        var _holdingButton = value.isPressed;

        print(string.Format("{0}, {1}", _selectedInteractable, _selectedInteractable != null));

        if (_selectedInteractable != null && _holdingButton)
        {
            _animator.Play("nen_jun_interact");
            _selectedInteractable.Interact(this);
        }

    }

    public void DisableInput()
    {
        _canAct = false; // Prevents the player from acting/moving
        rb.velocity = Vector2.zero; // Stops the player's movement immediately
    }

    public void EnableInput()
    {
        _canAct = true; // Allows the player to act/move again
    }

    #endregion

    #region Hiding

    public void OnHide(InputValue value)
    {
        var _pressing = value.isPressed;
        print(_pressing);

        if (currentHidingSpot != null) // Only allow hiding if in a hiding spot
        {

            if (_pressing) // Check if S key is pressed
            {
                isUnderLegsMode = false;
                isHiding = true; // Set hiding state
                SetAlpha(0.5f); // Set transparency to 50%
                spriteRenderer.sortingOrder = 2; // Change to the desired sorting order when hiding
                _animator.Play("nen_jun_hide");
                Debug.Log("Player is hiding in a hiding spot."); // Optional: log to console
            }
            else
            {
                isHiding = false; // Exit hiding state
                SetAlpha(1f);
                spriteRenderer.sortingOrder = normalSortingOrder;
                _animator.SetTrigger("Released");
                Debug.Log("Player is no longer hiding.");
            }
        }
    }

    public void EnterHidingSpot(HidingSpot hidingSpot)
    {
        currentHidingSpot = hidingSpot;
        Debug.Log("Entered hiding spot.");
    }

    public void ExitHidingSpot(HidingSpot hidingSpot)
    {
        if (currentHidingSpot == hidingSpot)
        {
            currentHidingSpot = null;
            Debug.Log("Exited hiding spot.");
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public bool IsHiding()
    {
        return isHiding;
    }

    #endregion

    #region Health

    private void SimulateDamage()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(50f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isHiding)
        {
            currentHealth -= damage;
        }
    }

    private void HandleDeath()
    {
        isHiding = false;
        underLeg = false;
        _animator.Play("nen_jun_ded");
        SaveSystem.LoadPlayer(gameObject);
    }

    #endregion

    #region Under the legs

    public void OnSpecial()
    {
        if (!isHiding)
        {
            isUnderLegsMode = !isUnderLegsMode;
            if (isUnderLegsMode)
                _animator.Play("nen_jun_looking_through_enter");
            else
                _animator.Play("nen_jun_looking_through_exit");

            UndertheLegStateChanged?.Invoke(isUnderLegsMode);
        }
    }

    public void UnderLegProcess()
    {
        SpecialOverlay.SetActive(isUnderLegsMode);

        if (isUnderLegsMode)
        {
            SpecialOverlay.transform.position = transform.position + Vector3.up * DetectionOffset.y;

            var sphereCast = Physics2D.OverlapCircleAll
                (transform.position + (new Vector3(DetectionOffset.x * -_facingAxis, DetectionOffset.y, 0))
                , DetectionRadius);

            Ghost _ghost = null;

            foreach (var col in sphereCast)
            {
                col.gameObject.TryGetComponent<Ghost>(out _ghost);
                if (_ghost)
                    _ghost.SetBeingSeen();

                print(col);
            }
        }
    }

    #endregion

    public void SetSavePoint(Vector3 position, float health)
    {
        savePoint = position;
        saveHealth = health;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere
            (transform.position + (new Vector3(DetectionOffset.x * -_facingAxis, DetectionOffset.y, 0))
            , DetectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCastLenght);

/*        if (SpecialOverlay)
        SpecialOverlay.transform.position = transform.position + Vector3.up * DetectionOffset.y;*/
    }

    void MovementAnimation()
    {
        transform.localScale = new Vector3(_facingAxis, 1, 1);

        if (rb.velocity.magnitude > 0)
            _animator.Play("nen_jun_run");

    }
}
