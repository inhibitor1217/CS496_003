using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour {

    public float Speed;

    public GameObject ThisObject;
    public Transform ThisTransform;

    public bool EnableEffects = true;

    public GameObject Explosion;
    public GameObject DestroySpark;
    public GameObject ReflectSpark;

    private const float MAX_LENGTH = 2.0f;

    private Vector2 _head, _tail;
    private float _direction;

    private bool _isCreating = true;
    private bool _isDestroying = false;

    private float _constantY;
    private float _constantXScale, _constantZScale;

    private Vector3 surfaceNormal;

    private GameObject source = null;
    private GameObject nextRay = null;

    public void setSource(GameObject obj) {
        source = obj;
    }

    private Vector2 unitVector(float angle) {
        float radAngle = Mathf.Deg2Rad * angle;
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }

    private void Start() {

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        float initialScale = transform.localScale.y;

        Vector2 position2D = new Vector2(transform.localPosition.x, transform.localPosition.z);

        _head = position2D + initialScale * Vector2.right;
        _tail = position2D - initialScale * Vector2.right;

        _direction = -transform.rotation.eulerAngles.y;

        _constantY = transform.localPosition.y;
        _constantXScale = transform.localScale.x;
        _constantZScale = transform.localScale.z;

    }

    private void LateUpdate() {

        if (!_isDestroying) {
            _head += unitVector(_direction) * Speed * Time.deltaTime;
        }

        if (!_isCreating) {
            _tail += unitVector(_direction) * Speed * Time.deltaTime;
        }

        if (_isCreating && Vector2.SqrMagnitude(_head - _tail) > MAX_LENGTH) {
            _isCreating = false;
        }

        if (_isDestroying && Vector2.Dot(_head - _tail, unitVector(_direction)) < 0.0f) {
            Destroy(gameObject);
            if(nextRay != null) {
                // nextRay.GetComponent<LazerController>()._isCreating = false;
            }
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

    private void OnTriggerEnter(Collider other) {

        if (!_isDestroying && other.gameObject != source) {
            if (other.tag == "Walls") {

                if (EnableEffects) {
                    Instantiate(DestroySpark, new Vector3(_head.x, _constantY, _head.y), Quaternion.identity);
                }

                Destroy(gameObject);

            } else if (other.tag == "Destroyable") {

                if (EnableEffects) {
                    Instantiate(DestroySpark, new Vector3(_head.x, _constantY, _head.y), Quaternion.identity);
                    Instantiate(Explosion, other.transform.position, other.transform.rotation);
                }

                Destroy(gameObject);
                Destroy(other.gameObject);

            } else if (other.tag == "Reflectable") {
                
                if(EnableEffects) {
                    Instantiate(ReflectSpark, new Vector3(_head.x, _constantY, _head.y), Quaternion.identity);
                }

                _isDestroying = true;
                
                Transform otherTransform = other.GetComponent<Transform>();

                float incidentAngle = transform.rotation.eulerAngles.y;
                float normalAngle = otherTransform.rotation.eulerAngles.y + 90.0f;

                float reflectAngle = 2.0f * normalAngle - incidentAngle + 180.0f;

                Vector2 NewRayPosition = _head + unitVector(_direction) * 0.02f * Speed;

                nextRay = Instantiate(ThisObject, new Vector3(NewRayPosition.x, ThisTransform.position.y, NewRayPosition.y),
                                               ThisTransform.rotation);

                nextRay.transform.Rotate(new Vector3(reflectAngle, 0.0f, 0.0f));
                nextRay.GetComponent<LazerController>().setSource(other.gameObject);

            } else if(other.tag == "Splittable") {

                if (EnableEffects) {
                    Instantiate(ReflectSpark, new Vector3(_head.x, _constantY, _head.y), Quaternion.identity);
                }

                Transform otherTransform = other.GetComponent<Transform>();

                float incidentAngle = transform.rotation.eulerAngles.y;
                float normalAngle = otherTransform.rotation.eulerAngles.y + 90.0f;

                float reflectAngle = 2.0f * normalAngle - incidentAngle + 180.0f;
                
                Vector2 NewRayPosition = _head + unitVector(_direction) * 0.03f * Speed;

                GameObject splittedRay = Instantiate(ThisObject, new Vector3(NewRayPosition.x, ThisTransform.position.y, NewRayPosition.y),
                                               ThisTransform.rotation);

                splittedRay.transform.Rotate(new Vector3(reflectAngle + 180.0f, 0.0f, 0.0f));
                splittedRay.GetComponent<LazerController>().setSource(other.gameObject);

            } else if(other.tag == "Player") {
                
                if(EnableEffects) {
                    Instantiate(DestroySpark, new Vector3(_head.x, _constantY, _head.y), Quaternion.identity);
                    Instantiate(Explosion, other.transform.position, other.transform.rotation);
                 }

                Destroy(gameObject);
                Destroy(other.gameObject);

            }
        }
    }

}
