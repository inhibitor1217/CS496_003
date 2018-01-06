using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour {

    public float Speed;

    private const float MAX_LENGTH = 2.0f;

    private Vector2 _head, _tail;    
    private float _theta;
    
    private bool _isCreating = true;

    private float _constantY;
    private float _constantXScale, _constantZScale;

    private Vector2 unitVector(float angle) {
        float radAngle = Mathf.Deg2Rad * angle;
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }

    private void Start() {

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        _theta = -eulerRotation.y;

        float initialScale = transform.localScale.y;

        Vector2 position2D = new Vector2(transform.localPosition.x, transform.localPosition.z);
        
        _head = position2D + initialScale * Vector2.right;
        _tail = position2D - initialScale * Vector2.right;

        _constantY      = transform.localPosition.y;
        _constantXScale = transform.localScale.x;
        _constantZScale = transform.localScale.z;

    }

    private void Update() {

        _head += Vector2.left * Speed * Time.deltaTime;

        if (!_isCreating) {
            _tail += Vector2.left * Speed * Time.deltaTime;
        }

        // finish creation
        if( Vector2.SqrMagnitude(_head - _tail) > MAX_LENGTH ) {
            _isCreating = false;
        }

        updateTransform();

    }

    // Abstract variables are now updated into transform of object.
    // DO NOT HANDLE TRANFORM VALUES IN OTHER METHODS.
    private void updateTransform() {

        Vector2 position2D = 0.5f * (_head + _tail);
        transform.localPosition = new Vector3(position2D.x, _constantY, position2D.y);

        transform.localScale = new Vector3(_constantXScale, 0.5f * Vector2.SqrMagnitude(_head - _tail), _constantZScale);

    }

}
