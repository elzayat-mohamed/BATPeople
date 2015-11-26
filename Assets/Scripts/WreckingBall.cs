using UnityEngine;
using System.Collections;

public class WreckingBall : MonoBehaviour
{

    public float angle = 90.0f;
    public float speed = 1.5f;

    Quaternion qStart, qEnd;
    private float startTime;
    void Start()
    {
        qStart = Quaternion.AngleAxis(angle, Vector3.forward);
        qEnd = Quaternion.AngleAxis(-angle, Vector3.forward);
    }
    void Update()
    {
        //if (IsActivated)
        {
            startTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(qStart, qEnd, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);
        }
    }
}
