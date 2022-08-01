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
    private float MoveSpeed = 4.0f;
    public Action_Manage bossActions;
    private bool animationCD = false;
    public float cdTime = 4.0f;
    Transform pTransform;
    // Start is called before the first frame update
    void Start()
    {
        bossActions = gameObject.GetComponent<Action_Manage>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, Player.transform.position));
        if (!animationCD)
        {
            animationCD = true;
            if (Vector3.Distance(transform.position, Player.transform.position) >= MinDist)
            {
                bossActions.Pressed_run();
                MoveToPlayer();
            }
            else
            {
                Debug.Log("Not Moving");
            }
            StartCoroutine(changeMode());
        }
    }

    private void MoveToPlayer()
    {
        transform.LookAt(Player.transform);
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        
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
