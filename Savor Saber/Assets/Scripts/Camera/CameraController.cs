using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraController : MonoBehaviour
{
    new private Transform camera;
    private Transform target;

    public GameObject debugSprite;

    public float radius = 5f;
    public float snapTime = 0.5f;
    public float maxSpeed = 1000f;
    private Vector2 currVelocity = Vector2.zero;
    private bool returning = false;
    private Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
            camera = GameObject.FindWithTag("MainCamera").transform;
    }

    public void SetTarget(GameObject target)
    {
        returning = false;
        this.target = target.transform;
    }
    public void ReleaseTarget()
    {
        returning = true;
        target = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            camera.position = new Vector3(transform.position.x, transform.position.y, camera.position.z);
            debugSprite.transform.position = transform.position;
            return;
        }
        if(returning)
        {
            //Vector3 point = camera.WorldToViewportPoint(target.position);
            //Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            //Vector3 destination = transform.position + delta;
            //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            var targetPos = new Vector2(transform.position.x, transform.position.y);// + (GetComponent<Rigidbody2D>().velocity * Time.deltaTime);
            var newPos = Vector2.SmoothDamp(camera.position, targetPos, ref currVelocity, snapTime, 30, Time.fixedDeltaTime);
            newPos += GetComponent<Rigidbody2D>().velocity * Time.deltaTime;
            camera.position = new Vector3(newPos.x, newPos.y, camera.position.z);
            debugSprite.transform.position = newPos;
            //lastPosition = transform.position;
            if (Vector2.Distance(camera.position, transform.position) < 0.1f)
            {
                returning = false;
                target = null;
            }
        }
        else
        {
            float distance = Vector2.Distance(transform.position, target.position);
            var targetPos = Vector2.Lerp(transform.position, target.position, Mathf.Clamp01(radius / distance));
            var newPos = Vector2.SmoothDamp(camera.position, targetPos, ref currVelocity, snapTime, maxSpeed, Time.deltaTime);
            camera.position = new Vector3(newPos.x, newPos.y, camera.position.z);
            debugSprite.transform.position = newPos;
        }
    }
}
