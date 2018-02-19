using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;
using System;
using System.IO;

public class PlayerCharacterController : MonoBehaviour {

    #region InspectorVariables

    [SerializeField]
    private float speed;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float endOfJumpGravity;

    [SerializeField]
    private float maxJumpHeight;

    [SerializeField]
    private LayerMask groundLayer;

    #endregion

    #region References

    private CharacterController characterController;
    private PlayerDefinition playerDefinition;
    private Controller controller;

    #endregion

    #region Private Variables

    private Vector3 velocity;
    private bool isJumping;

    #endregion

    void Awake () {
        PlayerDefinition playerDefinition = new PlayerDefinition
        {
            CharacterType = CharacterType.Red,
            Controller = new Controller(1)
        };
        SetPlayerDefinition(playerDefinition);

        characterController = GetComponent<CharacterController>();
    }

    void SetPlayerDefinition(PlayerDefinition playerDefinition)
    {
        this.playerDefinition = playerDefinition;
        controller = playerDefinition.Controller;
    }

    void Update () {
        Gravity();
         
        if (characterController.isGrounded && controller.GetButtonDown(Button.A))
        {
            Jump();
        }

        Move();
    }

    private void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;

        if (!isJumping && characterController.isGrounded)
        {
            velocity.y = gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        float horizontal = controller.GetAxis(Axis.LEFT_HORIZONTAL);
        Vector3 move = transform.forward * horizontal * speed * Time.deltaTime;

        characterController.Move(move + velocity * Time.deltaTime);
    }

    private void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        isJumping = true;
        
        //set vertical velocity to the velocity needed to reach maxJumpHeight
        velocity.y = Mathf.Sqrt(maxJumpHeight * -2f * gravity);

        yield return StartCoroutine(WaitUntilApexOfJumpOrReleaseButton());

        isJumping = false;

        yield return StartCoroutine(FallFaster());
    }

    private IEnumerator FallFaster()
    {
        while (!characterController.isGrounded)
        {
            velocity.y += endOfJumpGravity * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WaitUntilApexOfJumpOrReleaseButton()
    {
        float previousPositionY = transform.position.y;
        yield return null;

        while (!controller.GetButtonUp(Button.A) && previousPositionY < transform.position.y)
        {
            previousPositionY = transform.position.y;
            yield return null;
        }
    }
}
