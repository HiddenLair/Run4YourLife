using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

public class Laser : MonoBehaviour {

    public Transform xLaser;
    public float xSpeed;
    public Transform yLaser;
    public float ySpeed;

    public GameObject trapA;
    public GameObject floorTrapA;
    public GameObject ceilTrapA;
    public GameObject trapX;
    public GameObject floorTrapX;
    public GameObject ceilTrapX;

    private enum Type { Floor,Ceiling,Obstacle};
    private int setIndex = 1;
    private Type[] sets = new Type[] { Type.Floor, Type.Obstacle, Type.Ceiling };

    private PlayerDefinition playerDefinition;
    private Controller controller;

    private void Awake()
    {
        PlayerDefinition playerDefinition = new PlayerDefinition
        {
            CharacterType = CharacterType.Red,
            Controller = new Controller(1)
        };
        SetPlayerDefinition(playerDefinition);
    }

    void SetPlayerDefinition(PlayerDefinition playerDefinition)
    {
        this.playerDefinition = playerDefinition;
        controller = playerDefinition.Controller;
    }

    // Update is called once per frame
    void Update () {
        float xInput = controller.GetAxis(Axis.LEFT_HORIZONTAL);
        if (Mathf.Abs(xInput) > 0.2)
        {
            if(xInput > 0)
            {
                Vector3 temp = xLaser.position;
                temp.x = temp.x + xSpeed;
                xLaser.position = temp;
            }
            else
            {
                Vector3 temp = xLaser.position;
                temp.x = temp.x - xSpeed;
                xLaser.position = temp;
            }
        }
        float yInput = controller.GetAxis(Axis.LEFT_VERTICAL);
        if (Mathf.Abs(yInput) > 0.2)
        {
            if(yInput < 0)
            {
                Vector3 temp = yLaser.position;
                temp.y = temp.y + ySpeed;
                yLaser.position = temp;
            }
            else
            {
                Vector3 temp = yLaser.position;
                temp.y = temp.y - ySpeed;
                yLaser.position = temp;
            }

        }

        if (controller.GetButtonDown(Button.R))
        {
            setIndex = (setIndex - 1+sets.Length) % sets.Length;
        }

        if (controller.GetButtonDown(Button.L))
        {
            setIndex = (setIndex + 1) % sets.Length;
        }

        if (controller.GetButtonDown(Button.A))
        {
            if(sets[setIndex] == Type.Ceiling)
            {
                Vector3 temp= GetZone(Type.Ceiling);
                temp.y = temp.y - (ceilTrapA.GetComponent<Renderer>().bounds.size.y/2);
                Instantiate(ceilTrapA, temp, ceilTrapA.GetComponent<Transform>().rotation);
            }
            else if(sets[setIndex] == Type.Floor)
            {
                Vector3 temp = GetZone(Type.Floor);
                temp.y = temp.y + (floorTrapA.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(floorTrapA, temp, floorTrapA.GetComponent<Transform>().rotation);
            }
            else
            {
                Instantiate(trapA, yLaser.position, trapA.GetComponent<Transform>().rotation);
            }
        }

        if (controller.GetButtonDown(Button.X))
        {
            if (sets[setIndex] == Type.Ceiling)
            {
                Vector3 temp = GetZone(Type.Ceiling);
                temp.y = temp.y - (ceilTrapX.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(ceilTrapX, temp, ceilTrapX.GetComponent<Transform>().rotation);
            }
            else if (sets[setIndex] == Type.Floor)
            {
                Vector3 temp = GetZone(Type.Floor);
                temp.y = temp.y + (floorTrapX.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(floorTrapX, temp, floorTrapX.GetComponent<Transform>().rotation);
            }
            else
            {
                Instantiate(trapX, yLaser.position, trapX.GetComponent<Transform>().rotation);
            }
        }
    }

    Vector3 GetZone(Type type)
    {
        /*
        * Create the hit object
        * This will later hold the data for the hit
        * (location, collided collider etc.)
        */
        RaycastHit hit;
        /*
         * The ray length.
         * Modify it to change the length of the Ray.
         */
        float distance = 10f;
        /*
         * A variable to store the location of the hit.
         */
        Vector3 targetLocation = Vector3.zero;

        /*
         * Cast a raycast.
         * If it hits something:
         */
        switch (type)
        {
            case Type.Floor:
                {
                    if (Physics.Raycast(yLaser.position, Vector3.down, out hit, distance))
                    {
                        /*
                         * Get the location of the hit.
                         * This data can be modified and used to move your object.
                         */
                        targetLocation = hit.point;
                    }
                }
                break;
            case Type.Ceiling:
                {
                    if (Physics.Raycast(yLaser.position, Vector3.up, out hit, distance))
                    {
                        /*
                         * Get the location of the hit.
                         * This data can be modified and used to move your object.
                         */
                        targetLocation = hit.point;
                    }
                }
                break;
        }

        return targetLocation;
    }
}
