using UnityEngine;

public class Skeleton : EnemyBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (health <= 0)
        {
            
            anim.Play("death");
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("death")){
            KillEnemy();
        }
        if (health > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = transform.position.x - PlayerMovment.Instance.transform.position.x > 0;
            transform.position = Vector3.MoveTowards(transform.position, PlayerMovment.Instance.transform.position - new Vector3(0, 1.2f, 0), speed * Time.deltaTime);

            if (transform.position.y >= PlayerMovment.Instance.transform.position.y -  1.8f && transform.position.y <= PlayerMovment.Instance.transform.position.y + 1.0f && transform.position.x >= PlayerMovment.Instance.transform.position.x -  1.8f && transform.position.x <= PlayerMovment.Instance.transform.position.x +  1.8f)
            {
                anim.Play("attack");
            }
            else if (health > 0)
            {
                anim.Play("walk");
            }
        }
    }
}
