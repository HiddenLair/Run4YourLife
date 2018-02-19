using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

public class RigidbodyCharacterController : MonoBehaviour, IEventMessageTarget {
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
            Controller = new Controller(2)
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

    #region MessageHandling

    void IEventMessageTarget.Explosion()
    {
        Debug.Log(gameObject.name + " recieved an explosion!");
    }

    void IEventMessageTarget.Impulse(Vector3 force)
    {
        Debug.Log(gameObject.name + " recieved an impact!");

        rigidbody.isKinematic = false;

        Debug.Log(gameObject.name + " " + force);
        rigidbody.AddForce(new Vector3(1.0f, 0.0f, 0.0f) * 100.0f, ForceMode.Acceleration);

        //rigidbody.isKinematic = true;
    }

    #endregion

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
