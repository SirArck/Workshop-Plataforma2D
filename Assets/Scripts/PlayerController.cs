using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector2 _velocity;

    private Rigidbody2D _rb;
    private Collider2D _col;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    #region INPUTS

    private float _axisXInput;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _axisXInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Jump();
                break;
            case InputActionPhase.Canceled:
                _velocity.y = 0; 
                break;
        }
    }

    #endregion

    #region Cast

    [SerializeField] private LayerMask groundLayer;

    private void CastCollision()
    {
        _isOnGround = Physics2D.BoxCast(_col.bounds.center, new Vector2(_col.bounds.size.x, 0.1f), 0, Vector2.down, _col.bounds.size.y / 2, groundLayer);
    }

    #endregion

    #region Jump

    private bool _isOnGround = false;
    private float _jumpTime;

    [SerializeField] private float _jumpVelocity = 10;
    [SerializeField] private float _jumpTimer = 1f;

    private void Jump()
    {
        if (!_isOnGround) return;

        _velocity.y = _jumpVelocity;
        _jumpTime = Time.time;
    }

    #endregion

    #region Horizontal Movement

    [SerializeField] private float _speed = 5;

    private void HorizontalMove()
    {
        _velocity.x = _axisXInput * _speed * Time.deltaTime;
    }

    #endregion

    private void FixedUpdate()
    {
        HorizontalMove();

        CastCollision();

        if (Time.time > _jumpTime + _jumpTimer)
            _velocity.y = 0;

        _rb.velocity = _velocity;
    }
}
