using UnityEngine;

public class EnemyBehaviour : MonoBehaviour

{
    [Header("Enemy Stats")]
    [SerializeField] public float health;
    [SerializeField] public float speed;
    [SerializeField] public float xp;
    private bool gaveXp = false;

    protected Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (health <= 0)
        {
            if (!gaveXp)
            {
                PlayerMovment.Instance.currentXp += xp;
                gaveXp = true;
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    internal void KillEnemy()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.TryGetComponent<RangedAttack>(out var spell))
        {
            return;
        }

        health -= spell.GetDamage();
    }
}
