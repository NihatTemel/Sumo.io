using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    
    public Vector3 offset;

    [SerializeField] float smoothSpeed = 0.1f;

    private void Start()
    {
        offset = transform.position - target.position;        
    }

    private void LateUpdate()
    {
        SmoothFollow();
    }

    public void SmoothFollow()
    {
        Vector3 targetPos = target.position + offset;
        //targetPos.y = transform.position.y;
        Vector3 smoothFollow = Vector3.Lerp(transform.position,
        targetPos, smoothSpeed * Time.deltaTime);

        transform.position = smoothFollow;
        
    }
}
