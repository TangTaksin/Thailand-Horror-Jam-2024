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

    [SerializeReference] IInteractable _selectedInteractable;
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

    [SerializeReference] List<IInteractable> _InteractableList = new List<IInteractable>();
/*
    [SerializeReference] ItemData _heldItem;
    public ItemData heldItem
    {
        get { return _heldItem; }
        set
        {
            if (_heldItem == value) return;

            _heldItem = value;

            if (OnItemChange != null)
                OnItemChange?.Invoke(_heldItem);
        }
    }
*/
    float _InputAxis;
    float _facingAxis;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ChangePlayerCanActBool += SetActionBool;

    }

    private void OnDisable()
    {
        ChangePlayerCanActBool -= SetActionBool;

    }

    void SetActionBool(bool _value)
    {
        _canAct = _value;
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

/*        // Update animation parameters for movement and idle
        if (_InputVector2 != Vector2.zero)
        {
            _animator.SetFloat("Horizontal", _InputVector2.x);
            _animator.SetFloat("Vertical", _InputVector2.y);
            _animator.SetFloat("Speed", _InputVector2.sqrMagnitude);
            //AudioManager.Instance.PlayWalkingSFX();

            // Update the last direction when moving
            _lastMoveDirection = _InputVector2.normalized;
        }
        else
        {
            // Player is idle, use the last movement direction
            _animator.SetFloat("Horizontal", _lastMoveDirection.x);
            _animator.SetFloat("Vertical", _lastMoveDirection.y);
            _animator.SetFloat("Speed", 0);  // Set Speed to 0 to trigger idle
        }*/
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
            /*if (interact.isInteractable)
                continue;*/

            //Vector3 DifferenceToTarget = interact..position - currentPosition;
            //float DistanceToTarget = DifferenceToTarget.sqrMagnitude;

            /*if (DistanceToTarget < ClosestDistance)
            {
                ClosestDistance = DistanceToTarget;
                selectedInteractable = interact;
            }*/
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!_canAct)
            return;

        _holdingButton = value.isPressed;

 /*       if (_selectedInteractable != null)
        {
            if (_heldItem != null)
            {
                var used = false;

                var enoughStamina = (stamina >= heldItem.cost);

                if (enoughStamina)
                    used = _heldItem.UseItem(_selectedInteractable);

                if (used)
                {
                    DrainStamina(_heldItem.cost);
                    SetItem(null);
                }
                else
                    _selectedInteractable.Interact(this);

            }
            else
                _selectedInteractable.Interact(this);
        }
*/
    }
/*
    public void SetItem(ItemData _item)
    {
        if (heldItem)
        {
            if (_item == null)
            {
                heldItem = _item;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.pickUpWrong_sfx);
            }
        }
        else
        {
            heldItem = _item;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pickUp_sfx);

        }

    }
*/
    void ResetState(bool _bool)
    {

    }
}
