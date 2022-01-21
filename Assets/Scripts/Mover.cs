using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalSize;

    private BoxCollider2D _boxCollider;
    private Vector3 _moveDelta;
    private RaycastHit2D _hit;
    public float ySpeed = 0.4f;
    public float xSpeed = 0.7f;

    protected virtual void Start()
    {
        originalSize = transform.localScale;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // Reset _moveDelta
        _moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Swap sprite direction
        if (_moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (_moveDelta.x < 0)
            transform.localScale = new Vector3(-1 * originalSize.x, originalSize.y, originalSize.z);

        // Make sure we can move in thus direction by casting a box there first
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(0, _moveDelta.y), Mathf.Abs(_moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (_hit.collider == null)
        {
            // Move
            transform.Translate(0, _moveDelta.y * Time.deltaTime, 0);
        }

        // add push vector
        _moveDelta += pushDirection;

        // Reduce the push force every frame. based on recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRevoverySpeed);

        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(_moveDelta.x, 0), Mathf.Abs(_moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (_hit.collider == null)
        {
            // Move
            transform.Translate(_moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
