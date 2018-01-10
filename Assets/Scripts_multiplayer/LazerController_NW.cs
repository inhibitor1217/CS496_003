using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LazerController_NW : NetworkBehaviour {

    public float Speed;

    public GameObject ThisObject;
    public Transform ThisTransform;

    public GameObject Explosion;
    public GameObject DestroySpark;
    public GameObject ReflectSpark;

    private const float MAX_LENGTH = 2.0f;

    [SyncVar]
    private Vector2 _head, _tail;
    
    [SyncVar]
    private float _direction;

    [SyncVar]
    private bool _isCreating = true;

    [SyncVar]
    private bool _isDestroying = false;

    [SyncVar]
    private float _constantY;

    [SyncVar]
    private float _constantXScale, _constantZScale;
    
    [SyncVar]
    private GameObject source = null;
    
    [SyncVar]
    private GameObject nextRay = null;

    private StructuresLocationController_NW slc;

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
    
    private void SpawnNext(Vector2 NewRayPosition, float reflectAngle, GameObject source) {
        GameObject temp = Instantiate(ThisObject, new Vector3(NewRayPosition.x, ThisTransform.position.y, NewRayPosition.y),
                                               ThisTransform.rotation);

        temp.transform.Rotate(new Vector3(reflectAngle, 0.0f, 0.0f));
        temp.GetComponent<LazerController_NW>().setSource(source);

        NetworkServer.Spawn(temp);

        nextRay = temp;
    }

    public override void OnNetworkDestroy() {
        Instantiate(DestroySpark, new Vector3(_head.x, _constantY, _head.y), Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other) {

        if (!_isDestroying && other.gameObject != source) {
            if (other.tag == "Walls") {

                NetworkServer.Destroy(gameObject);

            } else if (other.tag == "Destroyable") {
                
                Destroyer d = other.gameObject.GetComponent(typeof(Destroyer)) as Destroyer;
                StructureController_NW mc = d.ParentObject.GetComponent(typeof(MirrorController_NW)) as StructureController_NW;

                mc.isAlive = false;

                NetworkServer.Destroy(gameObject);

            } else if (other.tag == "Reflectable") {

                _isDestroying = true;
                
                Transform otherTransform = other.GetComponent<Transform>();

                float incidentAngle = transform.rotation.eulerAngles.y;
                float normalAngle = otherTransform.rotation.eulerAngles.y + 90.0f;

                float reflectAngle = 2.0f * normalAngle - incidentAngle + 180.0f;

                Vector2 NewRayPosition = _head + unitVector(_direction) * 0.02f * Speed;

                SpawnNext(NewRayPosition, reflectAngle, (other.gameObject.GetComponent(typeof(NWSource)) as NWSource).ParentObject);

            } else if(other.tag == "Splittable") {

                Transform otherTransform = other.GetComponent<Transform>();

                float incidentAngle = transform.rotation.eulerAngles.y;
                float normalAngle = otherTransform.rotation.eulerAngles.y + 90.0f;

                float reflectAngle = 2.0f * normalAngle - incidentAngle + 180.0f;
                
                Vector2 NewRayPosition = _head + unitVector(_direction) * 0.03f * Speed;

                SpawnNext(NewRayPosition, reflectAngle + 180.0f, other.gameObject);

            } else if(other.tag == "Player") {

                NetworkServer.Destroy(other.gameObject);
                Destroy(gameObject);

            }
        }
    }

}
