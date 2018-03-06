using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

public class Laser : MonoBehaviour {

    public Transform laser;
    public Transform laserOff;
    public float timeBetweenY;
    public float xSpeed;

    public Phase phase;
    public GameObject skillA;
    public GameObject trapA;
    public GameObject skillX;
    public GameObject trapX;
    public GameObject skillY;
    public GameObject trapY;
    public GameObject skillB;
    public GameObject trapB;

    public enum Phase {Phase1,Phase2,Phase3 };
    private enum Type { TRAP,SKILL};
    private int setIndex = 0;
    private Type[] sets = new Type[] { Type.TRAP, Type.SKILL };

    private int yPosIndex = 0;
    private float lastYPos = 0;
    private float timerChangeBetweenY = 0;

    BossControlScheme bossControlScheme;
    private Animator anim;

    private void Awake()
    {
        bossControlScheme = GetComponent<BossControlScheme>();
        anim = GetComponent<Animator>();
        setIndex = (int)phase % sets.Length;
    }

    void Update () {

        MoveX(laser);
        MoveX(laserOff);

        MoveY();

        //We need to re-evaluate cause it may change inside MoveY
        if (laser.gameObject.activeInHierarchy)
        {
           SetTraps();
        }

        timerChangeBetweenY += Time.deltaTime;

        if (phase == Phase.Phase3)
        {
            if (bossControlScheme.nextSet.Started())
            {
                setIndex = (setIndex - 1 + sets.Length) % sets.Length;
            }

            if (bossControlScheme.previousSet.Started())
            {
                setIndex = (setIndex + 1) % sets.Length;
            }
        }
    }

    void MoveX(Transform t)
    {
        float xInput = bossControlScheme.moveTrapIndicatorHorizontal.Value();
        if (Mathf.Abs(xInput) > 0.2)
        {
            if (xInput > 0)
            {
                Vector3 temp = t.position;
                temp.x = temp.x + xSpeed;
                t.position = temp;
            }
            else
            {
                Vector3 temp = t.position;
                temp.x = temp.x - xSpeed;
                t.position = temp;
            }
        }
    }

    void MoveY()
    {
        RaycastHit[] posiblePlaces = GetZone();
        System.Array.Sort(posiblePlaces, (x, y) => y.distance.CompareTo(x.distance));
        if (posiblePlaces.Length > 0)
        {
            if (!laser.gameObject.activeInHierarchy)
            {
                laser.gameObject.SetActive(true);
                laserOff.gameObject.SetActive(false);
            }

            bool changed = false;
            laser.gameObject.SetActive(true);
            float yInput = bossControlScheme.moveTrapIndicatorVertical.Value();
            if (Mathf.Abs(yInput) > 0.2)
            {
                if (timerChangeBetweenY >= timeBetweenY)
                {
                    if (yInput < 0)
                    {
                        yPosIndex++;
                        changed = true;
                    }
                    else
                    {
                        yPosIndex--;
                        changed = true;
                    }
                    timerChangeBetweenY = 0.0f;
                }
            }
            yPosIndex = Mathf.Clamp(yPosIndex, 0, posiblePlaces.Length - 1);
            if (!changed)
            {
                for (int i = posiblePlaces.Length - 1; i >= 0; i--)
                {
                    //TODO: try to solve problem
                    //Problem with raycast, sometimes it takes the y a little different,
                    if (Mathf.Approximately(posiblePlaces[i].point.y,lastYPos) || posiblePlaces[i].point.y < lastYPos)
                    {
                        yPosIndex = i;
                        break;
                    }
                }
            }
            laser.position = posiblePlaces[yPosIndex].point;
            lastYPos = laser.position.y;
        }
        else
        {
            if (laser.gameObject.activeInHierarchy) {
                laserOff.gameObject.SetActive(true);
                Vector3 temp = laserOff.position;
                temp.y = laser.position.y;
                laserOff.position = temp;
                laser.gameObject.SetActive(false);
            }
        }
    }
    void SetTraps()
    {
        if (bossControlScheme.skill1.Started())
        {
            anim.SetTrigger("Casting");
            if (sets[setIndex] == Type.SKILL)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (skillA.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(skillA, temp, skillA.GetComponent<Transform>().rotation);
            }
            else if (sets[setIndex] == Type.TRAP)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (trapA.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(trapA, temp, trapA.GetComponent<Transform>().rotation);
            }
        }

        if (bossControlScheme.skill2.Started())
        {
            anim.SetTrigger("Casting");
            if (sets[setIndex] == Type.SKILL)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (skillX.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(skillX, temp, skillX.GetComponent<Transform>().rotation);
            }
            else if (sets[setIndex] == Type.TRAP)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (trapX.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(trapX, temp, trapX.GetComponent<Transform>().rotation);
            }
        }
        if (bossControlScheme.skill3.Started())
        {
            anim.SetTrigger("Casting");
            if (sets[setIndex] == Type.SKILL)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (skillY.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(skillY, temp, skillY.GetComponent<Transform>().rotation);
            }
            else if (sets[setIndex] == Type.TRAP)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (trapY.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(trapY, temp, trapY.GetComponent<Transform>().rotation);
            }
        }
        if (bossControlScheme.skill4.Started())
        {
            anim.SetTrigger("Casting");
            if (sets[setIndex] == Type.SKILL)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (skillB.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(skillB, temp, skillB.GetComponent<Transform>().rotation);
            }
            else if (sets[setIndex] == Type.TRAP)
            {
                Vector3 temp = laser.position;
                temp.y = temp.y + (trapB.GetComponent<Renderer>().bounds.size.y / 2);
                Instantiate(trapB, temp, trapB.GetComponent<Transform>().rotation);
            }
        }
    }

    RaycastHit[] GetZone()
    {
        /*
         * The ray length.
         * Modify it to change the length of the Ray.
         */
        float distance = 50f;
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
