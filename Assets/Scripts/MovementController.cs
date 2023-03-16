using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 3;
    private Vector3 velocityVector = Vector3.zero; // initial velocity
    public float maxVelocityChange = 3f;
    public Rigidbody rb;
    public float tiltAmount = 8;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Taking joystick input
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        //calculating velocity vectors
        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _zMovementInput;

        // calclulating final movement velocity vector
        Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;

        // apply moovement
        Move(_movementVelocityVector);

         transform.rotation = Quaternion.Euler(joystick.Vertical*speed*tiltAmount, 0 , -1*joystick.Horizontal*speed*tiltAmount);
    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    private void FixedUpdate() {
        if(velocityVector != Vector3.zero)
        {
            //Get Current Velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            //Apply a force 
            velocityChange.x = Mathf.Clamp(velocityChange.x, - maxVelocityChange, +maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, - maxVelocityChange, +maxVelocityChange);
            velocityChange.y = 0f;
            rb.AddForce(velocityChange, ForceMode.Acceleration);
        }

       
    } 
}
