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

    private Rigidbody2D rb;
    private float _inputAxis;
    float _facingAxis = 1;
    private Vector3 savePoint;
    private float saveHealth;

    bool _canAct;

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

    [Header("Under the Leg Setting")]
    private bool isUnderLegsMode = false; // Tracks whether Under the Legs mode is active
    public float DetectionRadius = 1f;
    public float DetectionOffset = 1f;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        normalSortingOrder = spriteRenderer.sortingOrder; // Store the initial sorting order
        InitializePlayer();
    }

    void Update()
    {
        HandleMovement();
        SimulateDamage();
        UnderLegProcess();

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

        DecideSelectedInteractable();
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
        if (isHiding || isUnderLegsMode) // Prevent movement while hiding
        {
            rb.velocity = Vector2.zero; // Stop player movement when hiding
            return;
        }

        rb.velocity = new Vector2(_inputAxis * moveSpeed, rb.velocity.y);

        // Flip player sprite based on movement direction
        transform.localScale = new Vector3(_facingAxis, 1, 1);
    }

    public void OnJump()
    {
    }

    #endregion

    #region Interact

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interact = collision.GetComponent<IInteractable>();

        print(interact + "entered");

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
        /*        if (!_canAct)
                    return;*/

        var _holdingButton = value.isPressed;

        print(string.Format("{0}, {1}", _selectedInteractable, _selectedInteractable != null));

        if (_selectedInteractable != null && _holdingButton)
        {
            _selectedInteractable.Interact(this);
        }

    }

    #endregion

    #region Hiding

    public void OnHide(InputValue value)
    {
        var _pressing = value.isPressed;

        if (currentHidingSpot != null) // Only allow hiding if in a hiding spot
        {

            if (_pressing) // Check if S key is pressed
            {
                isHiding = true; // Set hiding state
                SetAlpha(0.5f); // Set transparency to 50%
                spriteRenderer.sortingOrder = 2; // Change to the desired sorting order when hiding
                Debug.Log("Player is hiding in a hiding spot."); // Optional: log to console
            }
            else
            {
                isHiding = false; // Exit hiding state
                SetAlpha(1f);
                spriteRenderer.sortingOrder = normalSortingOrder;
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
        SaveSystem.LoadPlayer(gameObject);
    }

    #endregion

    #region Under the legs

    public void OnSpecial()
    {
        if (!isHiding)
        {
            isUnderLegsMode = !isUnderLegsMode;
            UndertheLegStateChanged?.Invoke(isUnderLegsMode);
            Debug.Log("Under the Legs mode activated.");
        }
    }

    public void UnderLegProcess()
    {
        if (isUnderLegsMode)
        {
            var sphereCast = Physics2D.OverlapCircleAll
                (transform.position + (Vector3.right * (DetectionOffset * -_facingAxis))
                , DetectionRadius);

            Ghost _ghost = null;

            foreach (var col in sphereCast)
            {
                col.gameObject.TryGetComponent<Ghost>(out _ghost);
                if (_ghost)
                    _ghost.SetBeingSeen();
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
            (transform.position + (Vector3.right * (DetectionOffset * -_facingAxis))
            , DetectionRadius);
    }
}
