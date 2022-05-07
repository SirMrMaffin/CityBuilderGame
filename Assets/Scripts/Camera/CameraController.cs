using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private float fastSpeed;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float movementTime;
    [SerializeField]
    private float rotationAmount;
    [SerializeField]
    private Vector3 zoomAmount;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    Plane plane = new Plane(Vector3.up, Vector3.zero);

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if(Input.GetButtonDown("Mouse Right"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray, out var entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if(Input.GetButton("Mouse Right"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray, out var entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if(Input.GetButtonDown("Mouse Middle"))
        {
            rotateStartPosition = Input.mousePosition;
        }
        
        if(Input.GetButton("Mouse Middle"))
        {
            rotateCurrentPosition = Input.mousePosition;

            var difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void HandleMovementInput()
    {
        if(Input.GetButton("Sprint"))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if(Input.GetAxis("Vertical") == 1)
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if(Input.GetAxis("Vertical") == -1)
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if(Input.GetAxis("Horizontal") == 1)
        {
            newPosition += (transform.right * movementSpeed);
        }
        if(Input.GetAxis("Horizontal") == -1)
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if(Input.GetAxis("Horizontal Rotation") == 1)
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if(Input.GetAxis("Horizontal Rotation") == -1)
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if(Input.GetAxis("Zoom") == 1)
        {
            newZoom += zoomAmount;
        }
        if(Input.GetAxis("Zoom") == -1)
        {
            newZoom -= zoomAmount;
        }

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime), Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime));
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
