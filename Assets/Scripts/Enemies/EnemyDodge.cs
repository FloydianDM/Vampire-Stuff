using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyDodgeEvent))]
public class EnemyDodge : MonoBehaviour
{   
    private Enemy _enemy;
    private EnemyDodgeEvent _enemyDodgeEvent;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemyDodgeEvent = GetComponent<EnemyDodgeEvent>();
    }

    private void OnEnable()
    {
        _enemyDodgeEvent.OnEnemyDodge += EnemyDodgeEvent_OnEnemyDodge;
    }

    private void OnDisable()
    {
        _enemyDodgeEvent.OnEnemyDodge -= EnemyDodgeEvent_OnEnemyDodge;
    }

    private void EnemyDodgeEvent_OnEnemyDodge(EnemyDodgeEvent @event, EnemyDodgeEventArgs args)
    {
        DodgeEnemy(args);
    }

    private void DodgeEnemy(EnemyDodgeEventArgs args)
    {
        StartCoroutine(DodgeEnemyRoutine(args));
    }

    private IEnumerator DodgeEnemyRoutine(EnemyDodgeEventArgs args)
    {
        _enemy.Rigidbody.velocity = Vector2.zero;

        float dodgeVectorX = Random.Range(15f + Mathf.Epsilon, 10f);
        float dodgeVectorY = Random.Range(10f, 15f + Mathf.Epsilon);
        Vector2 dodgeVector = new Vector3(dodgeVectorX, dodgeVectorY);

        _enemy.Rigidbody.AddForce(dodgeVector * args.DodgeThrust);

        yield return new WaitForSeconds(args.DodgeTime);

        StaticEventHandler.CallCombatNotifiedEvent("ENEMY DODGED", 0.5f);
    }
}
