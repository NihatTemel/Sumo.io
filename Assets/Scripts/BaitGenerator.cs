using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitGenerator : MonoBehaviour
{
    [SerializeField] BoxCollider MapBc;
    [SerializeField] GameObject Bait;
    [SerializeField] GameObject Tomato;
    [SerializeField] float SpawnHeigh;


    void Start()
    {

        StartCoroutine(BaitStartSpawn());
        InvokeRepeating("SpawnTomato", 2, 5);

    }



    IEnumerator BaitStartSpawn() 
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnBait();
        }
        InvokeRepeating("SpawnBait", 0, 1.5f);          // Invoke the "SpawnTomato" method with 2s initial delay and repeat every 5s
    }
    
    void SpawnTomato() 
    {
        float bound = MapBc.bounds.extents.x;               // find max bounds range for spawn positions;

        float posx = Random.Range(-bound, bound);
        float posz = Random.Range(-bound, bound);
        GameObject NewTomato = Instantiate(Tomato, new Vector3(posx, Random.Range(SpawnHeigh - 1.0f, SpawnHeigh + 2.0f), posz), Quaternion.Euler(new Vector3(-90, Random.Range(0, 360), 0))); // spawn random heigh , random rotation
        NewTomato.transform.parent = this.transform;
    }

    void SpawnBait() 
    {
        float bound = MapBc.bounds.extents.x;               // find max bounds range for spawn positions;

        float posx = Random.Range(-bound, bound);
        float posz = Random.Range(-bound, bound);
        GameObject NewBait = Instantiate(Bait, new Vector3(posx, Random.Range(SpawnHeigh-1.0f, SpawnHeigh+2.0f), posz), Quaternion.Euler(new Vector3(0,Random.Range(0,360),0))); // spawn random heigh , random rotation
        NewBait.transform.parent = this.transform;                                                                                                                                                                
    }

}
