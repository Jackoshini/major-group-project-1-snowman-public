using System;
using System.IO;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    internal static PlayerMovment Instance;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float limitDiagonal = 0.7f;
    [SerializeField] public int health = 3;
    [SerializeField] public float damage = 5.0f;
    [SerializeField] public int level = 1;
    [SerializeField] public float xpToNextLevel = 10.0f;
    [SerializeField] public float currentXp = 0.0f;
    [SerializeField] private GameObject playerSpell;
    [SerializeField] private float spellDelay;

    private Rigidbody2D body;
    private Transform staffPivot;
    private Transform spellSpawn;
    private float currentDelay;

    private Animator animator;
    private float horizontal;
    private float vertical;
    private float iFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        body = GetComponent<Rigidbody2D>();
        staffPivot = transform.Find("Wizard_Sprite/StaffPivot");
        spellSpawn = staffPivot.Find("Staff/SpellSpawn");
        animator = GetComponentInChildren<Animator>();
        if (File.Exists(Application.persistentDataPath + "/data.json"))
        {
            using StreamReader reader = new(Application.persistentDataPath + "/data.json");
            LevelData data = LevelData.Load(reader.ReadToEnd());
            health = data.PlayerHealth;
            damage = data.PlayerDamage;
            level = data.PlayerLevel;
            xpToNextLevel = data.PlayerXpToNextLevel;
            currentXp = data.PlayerCurrentXp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playWalkingAnimation();
        UpdateStaffRotation();

        if (Input.GetMouseButton(0) && currentDelay <= 0)
        {
            Instantiate(playerSpell, spellSpawn.position, staffPivot.localRotation);
            currentDelay = spellDelay;
        }
        else if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (iFrame > 0)
        {
            iFrame -= Time.deltaTime;
        }
        if (currentXp >= xpToNextLevel)
        {
            currentXp = 0;
            xpToNextLevel += 10.0f;
            level += 1;
            damage += 5.0f;
        }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= limitDiagonal;
            vertical *= limitDiagonal;
        }

        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private void playWalkingAnimation()
    {
        if (Input.GetKey("w") == true)
        {
            animator.Play("Player_Walk_Up");
        }
        else if (Input.GetKey("s") == true)
        {
            animator.Play("Player_Walk_Down");
        }
        else if (Input.GetKey("d") == true)
        {
            animator.Play("Player_Walk_Right");
        }
        else if (Input.GetKey("a") == true)
        {
            animator.Play("Player_Walk_Left");
        }
        else
        {
            animator.Play("Player_Idle");
        }
    }

    void UpdateStaffRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        float rotation = Mathf.Atan2(-0.5F + mousePos.y / Screen.height, -0.5F + mousePos.x / Screen.width) * Mathf.Rad2Deg;
        staffPivot.localRotation = Quaternion.Euler(rotation * Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && iFrame <= 0)
        {
            health--;
            iFrame = 2.0f;
        }
    }
}
