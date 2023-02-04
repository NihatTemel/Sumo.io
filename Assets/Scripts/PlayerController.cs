using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject triangleRoot;
    [Header("Player Basic Ýnfo")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotatespeed;
    [SerializeField] float ScaleUpgrade;
    public int pushPower;
    [SerializeField] float canMoveBackDuration;
    [SerializeField] GameObject Score3dText;
    
    bool canMove = true;
    [Header("VFX")]
    [SerializeField] GameObject WaterExplode;
    [SerializeField] GameObject FireTrail;

    [Header("")]
    [SerializeField] DynamicJoystick my_joystick;
    [SerializeField] GameObject EnemyRoot;

    [Header("CameraSettings")]
    [SerializeField] GameObject Camera;
    [SerializeField] float CameraUpgradeHeigh;

    [Header("Panels")]
    [SerializeField] GameObject FailPanel;
    [SerializeField] GameObject WinPanel;


    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    
    void Movement() 
    {
        rb.velocity = transform.forward.normalized * moveSpeed;

        if(my_joystick.Horizontal!= 0 && my_joystick.Vertical != 0)
        {                                                                      // take joystick inputs and turn it to rotation value. we need this work when joystick is on use.
            float angle = Mathf.Atan2(my_joystick.Horizontal, my_joystick.Vertical) * Mathf.Rad2Deg;       
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }
        triangleRoot.transform.rotation = transform.rotation;           // Triangle for direction sign

    }

    private void FixedUpdate()
    {
        if(canMove)
         Movement();
        GroundCheck();
        WinCheck();
    }

    void WinCheck() 
    {
        if (EnemyRoot.transform.childCount == 0)        // If there is no other enemies win the game
        {
            moveSpeed = 0;
            WinPanel.SetActive(true);
        }
    }

    void GroundCheck() 
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            if (hit.collider.CompareTag("water"))
            {
                GameFail();        // GroundCheck if there is water fail the game.
            }
            
        }
    }


    void GameFail() 
    {
        FailPanel.SetActive(true);
        rb.useGravity = true;
        GetComponent<PlayerController>().enabled = false;
    }


    void BaitUpgrade()                  // Interaction and upgrade with bait
    {
        transform.localScale += new Vector3(ScaleUpgrade, ScaleUpgrade, ScaleUpgrade);
        pushPower++;

        Camera.GetComponent<PlayerFollow>().offset.y += CameraUpgradeHeigh;                     // Camera heigh arrangement

    }


    IEnumerator TomatoControl()                 // when we hit the tomato we gain movespeed for 3 seconds and have a fire trail.
    {
        FireTrail.SetActive(true);
        moveSpeed += 2;
        yield return new WaitForSeconds(3);
        moveSpeed -= 2;
        FireTrail.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bait") 
        {
            Destroy(collision.gameObject);
            BaitUpgrade();
            Score3dText.GetComponent<Score3dtext>().ScoreShow(1,collision.transform);           // 3dscoretext value and position settings.
        }
        if (collision.collider.tag == "Tomato") 
        {
            Destroy(collision.gameObject);
            StartCoroutine(TomatoControl());
        }
        if (collision.collider.tag == "Enemy")                          // With enemy collision  
        {
            canMove = false;
            GameObject Enemy = collision.collider.gameObject;
            Enemy.GetComponent<EnemyController>().LastTouchedCharacter = this.gameObject;
            Enemy.GetComponent<EnemyController>().canMove = false;
            

            int PowerDif = pushPower - Enemy.GetComponent<EnemyController>().pushPower;         // Check which character is has more pushpower

            Vector3 pushDirection = (transform.position - Enemy.transform.position);            // The direction to push for enemy and player
            pushDirection.y = 0;
            pushDirection = pushDirection.normalized;
            int PlayerForcePower = 4 - PowerDif;                                            // Even the difference is too much we need to see a bit push back.
            if (PlayerForcePower < 1)
                PlayerForcePower = 1;

            rb.AddForce(pushDirection * PlayerForcePower, ForceMode.Impulse);

            int EnemyForcePower = 4 + PowerDif;                                         // Even the difference is too much we need to see a bit push back.
            if (EnemyForcePower < 1)
                EnemyForcePower = 1;

            Enemy.GetComponent<Rigidbody>().AddForce(-pushDirection * EnemyForcePower, ForceMode.Impulse);
            StartCoroutine(canMoveTrue(Enemy));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water") 
        {
            WaterExplode.SetActive(true);               // when hit the water enable the VFX
        }
    }



    IEnumerator canMoveTrue(GameObject enemy)                   // when we hit to an enemy both characters lose control for a short time.
    {
        yield return new WaitForSeconds(canMoveBackDuration);
        canMove = true;
        enemy.GetComponent<EnemyController>().canMove = true;
    }


   

}
