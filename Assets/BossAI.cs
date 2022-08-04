using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private NavMeshAgent agent;
    public float MoveDist = 4.0f;
    public float AttackDist = 0.7f;
    public float MoveSpeed = 4.0f;
    public Action_Manage bossActions;
    private bool animationCD = false;
    public float cdTime = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        bossActions = gameObject.GetComponent<Action_Manage>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animationCD)
        {
            animationCD = true;
            agent.SetDestination(player.transform.position);
            if (Vector3.Distance(transform.position, player.position) >= MoveDist)
            {
                bossActions.Pressed_run();
            }
            else if (Vector3.Distance(transform.position, player.position) >= AttackDist)
            {
                gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                bossActions.Pressed_attack_01();
            }
            StartCoroutine(changeMode());
        }
    }

    private void randomActions()
    {

    }

    IEnumerator changeMode()
    {
        yield return new WaitForSeconds(cdTime);
        animationCD = false;
    }
}
