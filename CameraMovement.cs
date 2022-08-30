using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Transform target;
    public float smoothing;
    public Vector2 minVal;
    public Vector2 maxVal;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != target.position){
            transform.position = Vector3.Lerp(transform.position, GetClampedPosition(), smoothing);
        }
    }

    private Vector3 GetClampedPosition(){
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        targetPosition.x = Mathf.Clamp(targetPosition.x, minVal.x, maxVal.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minVal.y, maxVal.y);
        return targetPosition;
    }
}
