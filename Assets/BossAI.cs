using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private float MinDist = 4.0f;
    [SerializeField]
    private float MoveSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Transform pTransform = Player.transform;
        transform.LookAt(pTransform);
        if (Vector3.Distance(transform.position, pTransform.position) >= MinDist)
        {

            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
    }
}
