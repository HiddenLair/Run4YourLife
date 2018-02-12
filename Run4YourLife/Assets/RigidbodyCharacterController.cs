using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

public class RigidbodyCharacterController : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpingForce;

    private bool isGrounded;

    //References
    private PlayerDefinition playerDefinition;
    private Controller controller;
    private new Rigidbody rigidbody;

    void Awake()
    {
        PlayerDefinition playerDefinition = new PlayerDefinition
        {
            CharacterType = CharacterType.Red,
            Controller = new Controller(1)
        };
        SetPlayerDefinition(playerDefinition);
        
        rigidbody = GetComponent<Rigidbody>();
    }

    void SetPlayerDefinition(PlayerDefinition playerDefinition)
    {
        this.playerDefinition = playerDefinition;
        controller = playerDefinition.Controller;
    }

    void Update()
    { 
        float horizontal = controller.GetAxis(Axis.LEFT_HORIZONTAL);

        rigidbody.velocity = GetVelocity(horizontal);

        if (isGrounded && controller.GetButtonDown(Button.A))
        {
            Jump();
        }
    }

    private Vector3 GetVelocity(float horizontal)
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.x = horizontal * speed * Time.deltaTime;
        return velocity;
    }

    private void Jump()
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = 0;
        rigidbody.velocity = velocity;
        rigidbody.AddForce(new Vector3(0, jumpingForce, 0), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
            Debug.Log("exit");
            isGrounded = false;
        }
    }
}
