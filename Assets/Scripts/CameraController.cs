using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour 
{
    private Animator animatorController;

    public Vector3 offset;
    private Transform target;
    private Transform camera;


    void Awake()
    {
        animatorController = this.GetComponent<Animator>();

        if (animatorController == null) throw new MissingReferenceException("Missing a reference to the PlayerAnimatorControler on the " + this.gameObject);

        camera = this.GetComponent<Transform>();

        var zoomOutState  = animatorController.GetBehaviour<AnimationStateEventDelegator>();
        zoomOutState.OnExit += OnZoomOutExit;
       
    }

    public void OnZoomOutExit()
    {
        follow = true;
        this.animatorController.enabled = false;
        this.startPosition = camera.position;
    }
    private bool follow = false;
    public void Follow(Transform target)
    {
        this.target = target;

    }
    private Vector3 startPosition;
    private float smoothTime = 1;
    private float smoothTimePassed = 0;

    void LateUpdate()
    {
        if (target &&  follow)
        {
            smoothTimePassed += Time.deltaTime;
            camera.position = Vector3.Lerp(startPosition, new Vector3(target.position.x + offset.x, offset.y, offset.z),smoothTimePassed / smoothTime );
        }
    }
    public void GameOver()
    {
        follow = false;

    }

    public void ZoomOut()
    {
        animatorController.SetTrigger("Action");
    }

    public void Restart()
    {
        follow = true;
    }
}
