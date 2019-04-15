using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Recordable : MonoBehaviour
{
    protected int recordingId;
    public Recorder recorder;
    public GameObject dummyType;

    protected virtual void Start()
    {
        GameObject gObj = gameObject;
        recordingId = recorder.GetObjectKey(dummyType, ref gObj);
    }

    protected virtual void InitRecordableState(RecordableState recordableState)
    {
        recordableState.position = transform.position;
        recordableState.rotation = transform.rotation.eulerAngles.z;
        recordableState.velocity = GetComponent<Rigidbody2D>().velocity;
    }

    protected virtual void LateUpdate()
    {
        RecordableState recordableState = new RecordableState();
        InitRecordableState(recordableState);
        recorder.FrameAddRecordableState(recordingId, recordableState);
    }
}

public class AnimatedRecordable : Recordable
{
    protected Animator animator;
    protected List<string> animTriggers = new List<string>();

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
    }

    protected override void InitRecordableState(RecordableState recordableState)
    {
        base.InitRecordableState(recordableState);
        recordableState.animTriggers = new List<string>(animTriggers);
        animTriggers.Clear();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected void ActivateAnimTrigger(string trigger)
    {
        animTriggers.Add(trigger);
        animator.SetTrigger(trigger);
    }
}



