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
    private Action_Manage bossActions;
    // Start is called before the first frame update
    void Start()
    {
        bossActions = gameObject.GetComponent<Action_Manage>();
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
            bossActions.Pressed_walk();
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

    private void randomActions()
    {

    }
}
