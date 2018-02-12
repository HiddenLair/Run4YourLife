using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameInput;

public class Boss : MonoBehaviour {
    //Shoot
    public GameObject shoot;
    public GameObject shoot2;
    public float rotationSpeed;
    public float bulletSpeed;
    public Transform bulletStartingPoint;
    public float reload;

    //Mele
    public GameObject mele;
    public Transform meleZone;
    public float meleReload;

    private Transform body;
    private float timer;
    private float meleTimer;
    private int shootMode = 0;
    private bool shootStillAlive = false;//Only for second shoot

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
        body = gameObject.GetComponent<Transform>();
        timer = reload;
        meleTimer = meleReload;
    }

    void SetPlayerDefinition(PlayerDefinition playerDefinition)
    {
        this.playerDefinition = playerDefinition;
        controller = playerDefinition.Controller;
    }

    // Update is called once per frame
    void Update() {
        if (controller.GetButtonDown(Button.START))
        {
            shootMode ^= 1;
        }
        switch (shootMode) {
            case 0:
                {
                    Shoot1(); 
                }
                break;
            case 1:
                {
                    Shoot2();
                }
                break;
        }
        if (Input.GetAxis("RT") > 0.2)
        {
            if (meleTimer >= meleReload)
            {
                var meleInst = Instantiate(mele, meleZone.position, mele.GetComponent<Transform>().rotation);
                Destroy(meleInst, 1.0f);
                meleTimer = 0.0f;
            }
        }
        meleTimer += Time.deltaTime;
    }

    void Shoot1()
    {
        float yInput = controller.GetAxis(Axis.RIGHT_VERTICAL);
        if (Mathf.Abs(yInput) > 0.2)
        {
            if (yInput < 0)
            {
                Quaternion temp = body.rotation * Quaternion.Euler(0, 0, rotationSpeed);
                body.rotation = temp;
            }
            else
            {
                Quaternion temp = body.rotation * Quaternion.Euler(0, 0, -rotationSpeed);
                body.rotation = temp;
            }

        }

        if (Input.GetAxis("LT") > 0.2)
        {
            if (timer >= reload)
            {
                var bulletInst = Instantiate(shoot, bulletStartingPoint.position, shoot.GetComponent<Transform>().rotation * body.rotation);
                bulletInst.GetComponent<Rigidbody>().velocity = bulletInst.GetComponent<Transform>().up * bulletSpeed;
                timer = 0;
            }
        }
        timer += Time.deltaTime;
    }

    void Shoot2()
    {
        if (Input.GetAxis("LT") > 0.2)
        {
            if (!shootStillAlive)
            {
                var bulletInst = Instantiate(shoot2, bulletStartingPoint.position, shoot2.GetComponent<Transform>().rotation * body.rotation);
                bulletInst.GetComponent<Rigidbody>().velocity = new Vector3(-bulletSpeed,0,0);
                bulletInst.GetComponent<MovingBullet>().SetCallback(gameObject);
                shootStillAlive = true;
            }
        }
    }

    public void SetShootStillAlive()
    {
        shootStillAlive = false;
    }
}
