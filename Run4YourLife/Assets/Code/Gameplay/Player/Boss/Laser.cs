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
    public float trapDelaySpawn;

    #region Public Trap CD
    public float globalTrapCD = 1.5f;
    public float trapACD = 5.0f;
    public float trapBCD = 5.0f;
    public float trapYCD = 5.0f;
    public float trapXCD = 5.0f;
    #endregion

    public enum Phase {Phase1,Phase2,Phase3 };
    private enum Type { TRAP,SKILL};
    private int setIndex = 0;
    private Type[] sets = new Type[] { Type.TRAP, Type.SKILL };

    private int yPosIndex = 0;
    private float lastYPos = 0;
    private float timerChangeBetweenY = 0.0f;

    #region Private Trap CD Timers
    private float globalTrapCD_Timer = 0.0f;
    private float trapACD_Timer = 0.0f;
    private float trapBCD_Timer = 0.0f;
    private float trapYCD_Timer = 0.0f;
    private float trapXCD_Timer = 0.0f;
    #endregion


    [HideInInspector]
    public bool isReadyForAction = true; //needed also for boss scripts
    BossControlScheme bossControlScheme;
    private Animator anim;

    private void Awake()
    {
        bossControlScheme = GetComponent<BossControlScheme>();
        anim = GetComponent<Animator>();
        setIndex = (int)phase % sets.Length;
        trapACD_Timer = trapACD;
        trapBCD_Timer = trapBCD;
        trapXCD_Timer = trapXCD;
        trapYCD_Timer = trapYCD;
    }

    void Update ()
    {
        //Add deltaTime to CD Timers
        globalTrapCD_Timer += Time.deltaTime;
        trapACD_Timer += Time.deltaTime;
        trapBCD_Timer += Time.deltaTime;
        trapXCD_Timer += Time.deltaTime;
        trapYCD_Timer += Time.deltaTime;

        isReadyForAction = anim.GetCurrentAnimatorStateInfo(0).IsName("move");

        MoveX(laser);
        MoveX(laserOff);

        MoveY();

        //We need to re-evaluate cause it may change inside MoveY
        if (laser.gameObject.activeInHierarchy && isReadyForAction)
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
                temp.x = temp.x + xSpeed * Time.deltaTime;
                t.position = temp;
            }
            else
            {
                Vector3 temp = t.position;
                temp.x = temp.x - xSpeed * Time.deltaTime;
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
        if (globalTrapCD_Timer > globalTrapCD)
        {
            if (bossControlScheme.skill1.Started() && (trapACD_Timer > trapACD))
            {
                trapACD_Timer = 0.0f;
                SetElement(trapA, skillA);
            }
            else if (bossControlScheme.skill2.Started() && (trapXCD_Timer > trapXCD))
            {
                trapXCD_Timer = 0.0f;
                SetElement(trapX, skillX);
            }
            else if (bossControlScheme.skill3.Started() && (trapYCD_Timer > trapYCD))
            {
                trapYCD_Timer = 0.0f;
                SetElement(trapY, skillY);
            }
            else if (bossControlScheme.skill4.Started() && (trapBCD_Timer > trapBCD))
            {
                trapBCD_Timer = 0.0f;
                SetElement(trapB, skillB);
            }
        }
    }

    void SetElement(GameObject trap, GameObject skill)
    {
        globalTrapCD_Timer = 0.0f;

        anim.SetTrigger("Casting");
        isReadyForAction = false;
        if (sets[setIndex] == Type.SKILL)
        {
            Vector3 temp = laser.position;
            temp.y = temp.y + (skill.GetComponent<Renderer>().bounds.size.y / 2);
            Instantiate(skill, temp, skill.GetComponent<Transform>().rotation);
        }
        else if (sets[setIndex] == Type.TRAP)
        {
            Vector3 temp = laser.position;
            temp.y = temp.y + (trap.GetComponent<Renderer>().bounds.size.y / 2);
            GameObject g = Instantiate(trap, temp, trap.GetComponent<Transform>().rotation);
            g.GetComponent<Collider>().enabled = false;
            Color actualC = g.GetComponent<Renderer>().material.color;
            actualC.a = 0;
            g.GetComponent<Renderer>().material.color = actualC;
            StartCoroutine(SpawnElementDelayed(g, trapDelaySpawn));
        }
    }

    IEnumerator SpawnElementDelayed(GameObject g, float time)
    {
        float fps = 1 / Time.deltaTime;
        float alphaPerFrame = 1 / (time * fps);
        Color temp = g.GetComponent<Renderer>().material.color;
        while (temp.a < 1)
        {
            temp.a += alphaPerFrame;
            g.GetComponent<Renderer>().material.color = temp;
            yield return 0;//Wait 1 frame
        }
        g.GetComponent<Collider>().enabled = true;
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
