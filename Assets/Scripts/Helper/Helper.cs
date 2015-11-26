using UnityEngine;
using System.Collections;
using System;

public class Helper : MonoBehaviour 
{
    public static Helper Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Delay(float time, Action action)
    {
        StartCoroutine(DelayAction(time, action));
    }
    public void DelayLoop(float time, Action action)
    {
        StartCoroutine(LoopDelay(time, action));
    }
    public void DelayFrames(int numberOfFrames, Action action)
    {
        StartCoroutine(DelayFramesAction(numberOfFrames, action));
    }

    private IEnumerator DelayAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        
        if(action == null)
        {
            throw new Exception("Action sent to Helper.Delay is NULL");
        }
        else
        {
            action();
        }

    }

    private IEnumerator DelayFramesAction(int numberOfFrames, Action action)
    {
        int passedFrames = 0;
        while (passedFrames < numberOfFrames)
        {
            yield return null;
            passedFrames++;
        }

        if (action == null)
        {
            throw new Exception("Action sent to Helper.Delay is NULL");
        }
        else
        {
            action();
        }

    }

    private IEnumerator LoopDelay(float time, Action action)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (action == null)
            {
                throw new Exception("Action sent to Helper.DelayLoop is NULL");
            }
            else
            {
                action();
            }
        }
    }

}
