using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

public class Laser : MonoBehaviour {

    public Transform laser;
    public float timeBetweenY;
    public float xSpeed;

    public GameObject trapA;
    public GameObject floorTrapA;
    public GameObject ceilTrapA;
    public GameObject trapX;
    public GameObject floorTrapX;
    public GameObject ceilTrapX;

    private enum Type { Type1,Type2};
    private int setIndex = 0;
    private Type[] sets = new Type[] { Type.Type1, Type.Type2 };

    private int yPosIndex = 0;
    private float timerChangeBetweenY = 0;

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
                Vector3 temp = laser.position;
                temp.x = temp.x + xSpeed;
                laser.position = temp;
            }
            else
            {
                Vector3 temp = laser.position;
                temp.x = temp.x - xSpeed;
                laser.position = temp;
            }
        }

        RaycastHit[] posiblePlaces = GetZone();
        System.Array.Sort(posiblePlaces, (x, y) => y.distance.CompareTo(x.distance));
        if (posiblePlaces.Length > 0)
        {
            laser.gameObject.SetActive(true);
            float yInput = controller.GetAxis(Axis.LEFT_VERTICAL);
            if (Mathf.Abs(yInput) > 0.2)
            {
                if (timerChangeBetweenY >= timeBetweenY)
                {
                    if (yInput < 0)
                    {
                        yPosIndex++;
                    }
                    else
                    {
                        yPosIndex--;
                    }
                    timerChangeBetweenY = 0.0f;
                }
            }
            yPosIndex = Mathf.Clamp(yPosIndex, 0, posiblePlaces.Length - 1);

            laser.position = posiblePlaces[yPosIndex].point;
        }
        else
        {
            laser.gameObject.SetActive(false);
        }

        timerChangeBetweenY += Time.deltaTime;


        if (controller.GetButtonDown(Button.RB))
        {
            setIndex = (setIndex - 1+sets.Length) % sets.Length;
        }

        if (controller.GetButtonDown(Button.LB))
        {
            setIndex = (setIndex + 1) % sets.Length;
        }

        if (controller.GetButtonDown(Button.A))
        {
            if(sets[setIndex] == Type.Type2)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (ceilTrapA.GetComponent<Renderer>().bounds.size.y/2);
                Instantiate(ceilTrapA, temp, ceilTrapA.GetComponent<Transform>().rotation);
            }
            else if(sets[setIndex] == Type.Type1)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (floorTrapA.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(floorTrapA, temp, floorTrapA.GetComponent<Transform>().rotation);
            }
        }

        if (controller.GetButtonDown(Button.X))
        {
            if (sets[setIndex] == Type.Type2)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (ceilTrapX.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(ceilTrapX, temp, ceilTrapX.GetComponent<Transform>().rotation);
            }
            else if (sets[setIndex] == Type.Type1)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (floorTrapX.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(floorTrapX, temp, floorTrapX.GetComponent<Transform>().rotation);
            }
        }
    }

    RaycastHit[] GetZone()
    {
        /*
         * The ray length.
         * Modify it to change the length of the Ray.
         */
        float distance = 10f;
        /*
         * A variable to store the location of the hit.
         */
        RaycastHit[] targetLocation;

        /*
         * Cast a raycast.
         * If it hits something:
         */
        float zDistance =laser.position.z - Camera.main.transform.position.z;
        Vector3 pos = new Vector3(laser.position.x,Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance)).y, laser.position.z);
        targetLocation = Physics.RaycastAll(pos,Vector3.down,distance,LayerMask.GetMask("Ground"));

        return targetLocation;
    }
}
