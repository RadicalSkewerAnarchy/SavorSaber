﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component used to control the camera.
/// Attach to the object you would like to use as the player NOT the camera
/// Make sure the camera is tagger "MainCamera"
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class CameraController : MonoBehaviour
{
    private bool _detatched;
    public bool Detatched
    {
        get => _detatched;
        set
        {
            _detatched = value;
            returning = true;
        }
    }
    public float returnTime = 0.5f;
    public float maxReturnSpeed = 1000f;
    public Vector2 deadzone = new Vector2(1.5f, 1.25f);

    new private Transform camera;
    private Transform target = null;
    private float radius = 5f;
    private float snapTime = 0.5f;
    private float maxSpeed = 1000f;
    private Vector2 currVelocity = Vector2.zero;
    private bool returning = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").transform;
    }
    /// <summary> Set a point of interest as the current target </summary>
    public void SetTarget(GameObject target, float maxPull, float maxSpeed, float snapTime)
    {
        returning = false;
        radius = maxPull;
        this.maxSpeed = maxSpeed;
        this.snapTime = snapTime;
        this.target = target.transform;
    }
    /// <summary> Release the camera from it's current point of interest </summary>
    public void ReleaseTarget()
    {
        returning = true;
        target = null;
    }

    public IEnumerator MoveToPointLinearCr(Vector2 point, float speed)
    {
        while (Vector2.Distance(camera.position, point) > speed * Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            var newPos = Vector2.MoveTowards(camera.position, point, speed * Time.fixedDeltaTime);
            camera.position = new Vector3(newPos.x, newPos.y, camera.position.z);
        }
        camera.position = new Vector3(point.x, point.y, camera.position.z);
    }
    public IEnumerator MoveToPointSmoothCr(Vector2 point, float maxSpeed, float snapTime)
    {
        Vector2 currVelocity = Vector2.zero;
        while (Vector2.Distance(camera.position, point) > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            var newPos = Vector2.SmoothDamp(camera.position, point, ref currVelocity, snapTime, maxSpeed, Time.fixedDeltaTime);
            camera.position = new Vector3(newPos.x, newPos.y, camera.position.z);
        }
        camera.position = new Vector3(point.x, point.y, camera.position.z);
    }

    // FixedUpdate removes jitter to Rigidbody movement
    void FixedUpdate()
    {
        Debug.Log("In camera controller");

        //what the fuck are you doing so far away
        if(Vector3.Distance(camera.position, transform.position) > 50f)
        {
            camera.position = new Vector3(transform.position.x, transform.position.y, camera.position.z);
        }
        if (Detatched)
            return;
        if (target == null)
        {
            if (returning)
                ReturnToPlayer();
            else
                FollowPlayer();
        }
        else //Go to current point of interest
            GoToPointOfInterest();
    }
    /// <summary> Follows the player exactly </summary>
    private void FollowPlayer()
    {
        camera.position = new Vector3(transform.position.x, transform.position.y, camera.position.z);
    }
    /// <summary> Follows the player with a configurable deadzone </summary>
    private void FollowPlayerWithDeadZone()
    {
        float xDist = camera.position.x - transform.position.x;
        float yDist = camera.position.y - transform.position.y;
        if (Mathf.Abs(xDist) > deadzone.x)
            camera.position = new Vector3(transform.position.x + (xDist >= 0 ? 1 : -1) * deadzone.x, camera.position.y, camera.position.z);
        if (Mathf.Abs(yDist) > deadzone.y)
            camera.position = new Vector3(camera.position.x, transform.position.y + (yDist >= 0 ? 1 : -1) * deadzone.y, camera.position.z);
    }
    /// <summary> Return to the player after the camera is released </summary>
    private void ReturnToPlayer()
    {
        var targetPos = new Vector2(transform.position.x, transform.position.y);
        var newPos = Vector2.SmoothDamp(camera.position, targetPos, ref currVelocity, returnTime, maxReturnSpeed, Time.fixedDeltaTime);
        // Add the player's current velocity step in to make sure the camera catches up if they are moveing
        newPos += GetComponent<Rigidbody2D>().velocity * Time.fixedDeltaTime;
        camera.position = new Vector3(newPos.x, newPos.y, camera.position.z);
        // Return to default behavior if close enough to the player
        if (Vector2.Distance(camera.position, transform.position) < 0.01f)
            returning = false;
    }
    /// <summary> Go to a point of interest </summary>
    private void GoToPointOfInterest()
    {
        //Calculate the target point (bounded by the radius)
        float distance = Vector2.Distance(transform.position, target.position);
        var targetPos = Vector2.Lerp(transform.position, target.position, Mathf.Clamp01(radius / distance));
        //Move towards the target point (with smoothing)
        SmoothStep(targetPos, this.snapTime, this.maxSpeed);
    }

    private void SmoothStep(Vector2 targetPos, float time, float maxSpeed)
    {
        var newPos = Vector2.SmoothDamp(camera.position, targetPos, ref currVelocity, time, maxSpeed, Time.fixedDeltaTime);
        camera.position = new Vector3(newPos.x, newPos.y, camera.position.z);
    }
}
