using UnityEngine;

public class Necromancer : EnemyBehaviour
{
    [Header("Spell Attack")]
    [SerializeField]
    private GameObject spellSpawn;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField]
    private float castingDistance;
    [SerializeField]
    private float castingDelay;

    private float lastCast = 0;

    new void Update()
    {
        base.Update();

        if (health <= 0)
        {
            anim.SetBool("Dead", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Necromancer_Death"))
        {
            KillEnemy();
        }
        if (health > 0)
        {
            lastCast += Time.deltaTime;

            transform.localScale = new Vector3(transform.position.x - PlayerMovment.Instance.transform.position.x > 0 ? 1F : -1F, 1, 1);
            if (Vector2.Distance(transform.position, PlayerMovment.Instance.transform.position) > castingDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    PlayerMovment.Instance.gameObject.transform.position,
                    speed * Time.deltaTime
                );
            }
            else if (lastCast > castingDelay)
            {
                anim.SetTrigger("Attacking");
                lastCast = 0;
            }
        }
    }

    public void CastSpell()
    {
        Instantiate(spellPrefab, spellSpawn.transform.position, Quaternion.identity);
    }
}
