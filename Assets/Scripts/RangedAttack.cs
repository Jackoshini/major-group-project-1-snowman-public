using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField]
    private ProjectileTeam team;
    [SerializeField]
    private float damage; // only used for player
    [SerializeField]
    private float speed;
    [SerializeField]
    [Range(-180F, 180F)]
    private float rotationAdjustment;

    private new Renderer renderer;
    private Vector3 translation;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        if (team == ProjectileTeam.Enemy)
        {
            translation = PlayerMovment.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(translation.y, translation.x) * Mathf.Rad2Deg, Vector3.forward);
        }
        else
        {
            translation = new Vector3(Mathf.Cos(transform.eulerAngles.z), Mathf.Sin(transform.eulerAngles.z), 0);
        }
    }

    void Update()
    {
        if (!renderer.isVisible)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    public float GetDamage()
    {
        if (team == ProjectileTeam.Player)
        {
            return PlayerMovment.Instance.damage;
        }
        else
        {
            return damage;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (team)
        {
            case ProjectileTeam.Player:
                if (collider.TryGetComponent<EnemyBehaviour>(out var enemy))
                {
                    Destroy(gameObject);
                }
                break;

            case ProjectileTeam.Enemy:
                if (collider.GetComponent<PlayerMovment>() != null)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private enum ProjectileTeam
    {
        Player,
        Enemy
    }
}
