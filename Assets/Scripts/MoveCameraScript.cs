using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraScript : MonoBehaviour
{
    [SerializeField] float smoothness = 1;
    private Vector3 desiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        desiredPosition = new Vector3(0, 0, -10);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothness);
    }

    public void SetDesiredPosition(float x, float y)
    {
        desiredPosition = new Vector3(x, y, -10);
    }
}