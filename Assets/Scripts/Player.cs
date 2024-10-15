using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D _body;
    Animator _animator;

    Vector2 _lastMoveDirection = Vector2.down;

    bool _canAct = true;
    public bool _holdingButton;

    float _movementSpeed;
    public float baseWalkSpeed;

    public delegate void OnObjectChangeDelegate(object newVal);
    public static OnObjectChangeDelegate OnInteractableChange;
    public static OnObjectChangeDelegate OnItemChange;

    public delegate void PlayerBoolEvent(bool _value);
    public static PlayerBoolEvent ChangePlayerCanActBool;

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

    float _InputAxis;
    float _facingAxis;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        DialogueManager.DialogueEnd += OnResume;
        DialogueManager.DialogueCalled += OnPause; 
    }

    private void OnDisable()
    {
        DialogueManager.DialogueEnd -= OnResume;
        DialogueManager.DialogueCalled -= OnPause;
    }

    private void Update()
    {
        UpdateMovement();

        DecideSelectedInteractable();
    }

    private void UpdateMovement()
    {
        _movementSpeed = baseWalkSpeed;
        _body.position += Vector2.right * (_InputAxis * _movementSpeed) * Time.deltaTime;
    }

    public void OnMove(InputValue _value)
    {
        if (!_canAct)
            return;

        _InputAxis = _value.Get<float>();

        _facingAxis = Mathf.Lerp(_facingAxis, _InputAxis, _InputAxis);
    }

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
        if (!_canAct)
            return;

        _holdingButton = value.isPressed;

        print(string.Format("{0}, {1}", _selectedInteractable, _selectedInteractable != null));

        if (_selectedInteractable != null && _holdingButton)
        {
            _selectedInteractable.Interact(this);
        }

    }

    void SetActable(bool _value)
    {
        _canAct = _value;
        _InputAxis = 0;
    }

    void OnPause()
    {
        SetActable(false);
    }

    void OnResume()
    {
        SetActable(true);
    }
}
