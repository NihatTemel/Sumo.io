using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyController : MonoBehaviour
{
    [SerializeField] BoxCollider MapBc;
    [Header("Enemy Basic Ýnfo")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotatespeed;
    [SerializeField] float ScaleUpgrade;
    public int pushPower;
    public GameObject LastTouchedCharacter;

    public bool canMove = true;
    bool canTargetPlayer = true;
    Vector3 MoveDirection;
    [SerializeField] Transform Player_t;

    [Header("VFX")]
    [SerializeField] GameObject WaterExplode;
    [SerializeField] GameObject FireTrail;
    [SerializeField] GameObject Score3dText;


    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MoveDirection = FindDirectionPoint();
    }


    void EnemyMovement() 
    {
        if (Vector3.Distance(transform.position, MoveDirection) < 0.5f) 
        {
            MoveDirection = FindDirectionPoint();
            canTargetPlayer = true;
        }

        rb.velocity = transform.forward.normalized * moveSpeed;
        LookAtSmoothly();



    }


    void LookAtSmoothly() 
    {
        Vector3 targetDirection = (MoveDirection - transform.position).normalized;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation.x = transform.rotation.x;
        //transform.DORotateQuaternion(targetRotation, 0.1).SetEase(Ease.InOutSine);
        //transform.LookAt(MoveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotatespeed);
    }


    Vector3 FindDirectionPoint() 
    {
        float bound = MapBc.bounds.extents.x;               // find max bounds range for spawn positions;
        float posx = Random.Range(-bound, bound);
        float posz = Random.Range(-bound, bound);

        return new Vector3(posx, transform.position.y, posz);
    }

    void PlayerTargetControl() 
    {
        if (Vector3.Distance(transform.position, Player_t.position) < 4 && canTargetPlayer) 
        {
            canTargetPlayer = false;
            if (Random.Range(0, 100) > 30)                      // When enemy character is close to Player character closer than 4 unit. %70 chance to change movedirection to player for amoment not permanent.
            {
                MoveDirection.x = Player_t.position.x;
                MoveDirection.z = Player_t.position.z;
            }

        }
    }

    private void FixedUpdate()
    {
        if (canMove) 
            EnemyMovement();
        GroundCheck();
        PlayerTargetControl();
    }

    void GroundCheck()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            // Raycast hit something, check if it's the ground
            if (hit.collider.CompareTag("water"))
            {
                EnemyDead();        
            }

        }
    }
    void EnemyDead()
    {
        rb.useGravity = true;
        if (LastTouchedCharacter.tag == "Player") 
        {
            LastTouchedCharacter.GetComponent<PlayerController>().pushPower += (pushPower+2);
            Score3dText.GetComponent<Score3dtext>().ScoreShow(pushPower + 2, Player_t.transform);
        }
        else
            LastTouchedCharacter.GetComponent<EnemyController>().pushPower += (pushPower+2);
        GetComponent<EnemyController>().enabled = false;
        GetComponent<Collider>().isTrigger = true;
        Destroy(this.gameObject, 1.5f);
    }

    void BaitUpgrade()                  // Interaction and upgrade with bait
    {
        transform.localScale += new Vector3(ScaleUpgrade, ScaleUpgrade, ScaleUpgrade);
        pushPower++;
    }
    IEnumerator canMoveTrue(GameObject enemy)                   // when  hit to an other enemy both characters lose control for a short time.
    {
        yield return new WaitForSeconds(0.6f);
        canMove = true;
        enemy.GetComponent<EnemyController>().canMove = true;
    }

    IEnumerator TomatoControl()                          // when hit the tomato enemies gain movespeed for 3 seconds and have a fire trail.
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
            BaitUpgrade();                              // Upgrade character with size and power
        }
        if (collision.collider.tag == "Tomato")
        {
            Destroy(collision.gameObject);  
            StartCoroutine(TomatoControl());                 // Boost character for a short time.
        }
        if (collision.collider.tag == "Enemy")                     // Collision with 2 enemies.
        {

            canMove = false;
            GameObject OtherEnemy = collision.collider.gameObject;
            LastTouchedCharacter = OtherEnemy;

            collision.collider.gameObject.GetComponent<EnemyController>().canMove = false;
           

            Vector3 pushDirection = (transform.position - collision.gameObject.transform.position);
            pushDirection.y = 0;
            pushDirection = pushDirection.normalized;

            int PowerDif = pushPower - OtherEnemy.GetComponent<EnemyController>().pushPower;            // Find power difference and arrange the direction by using it.
            int EnemyForcePower = 4 - PowerDif;
            if (EnemyForcePower < 1)
                EnemyForcePower = 1;


            rb.AddForce(pushDirection * EnemyForcePower, ForceMode.Impulse);
            StartCoroutine(canMoveTrue(collision.collider.gameObject));                 // After collision lose control for a short time.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water")
        {
            WaterExplode.SetActive(true);
        }
    }

}
