using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwlinatorAI : Owl
{
    public GameObject projectilePrefab;

    public GameObject bossHealthBar;
    public Image bossHealthBarImage;

    public float projectileDamage;
    public float projectileSpeed;

    public GameObject bubble;

    private Rigidbody2D owlRigidbody;

    public int chargeCooldown = 5;
    private float chargeTimer;

    private Vector3 originalPosition;

    private Vector3 targetPosition;

    private bool charging;
    private bool returning;

    private float bubbleVulnerableTimer;
    private float currentBubbleVulnerableTimer;

    private float burstShootCooldown;
    private float burstShootCurrentTime;
    private bool burstShooting;
    private int shotCount;

    private float shootCooldown;
    private float shootTimer;

    private GameManager m_gameManager = default;
    private PlayerController m_playerController = default;

    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        m_gameManager = GameObject.FindObjectOfType<GameManager>();
        m_playerController = GameObject.FindObjectOfType<PlayerController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        chargeTimer = 2; //Random.Range(chargeCooldown, chargeCooldown + 3);
        charging = false;
        returning = false;
        originalPosition = transform.position;
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies
        bubbleVulnerableTimer = 2f;
        burstShootCooldown = 5f;
        burstShootCurrentTime = burstShootCooldown;
        burstShooting = false;
        shotCount = 0;
        shootCooldown = 0.3f;
        shootTimer = shootCooldown;
        originalHitPoints = hitPoints;

        bossHealthBar = player.transform.Find("UI").gameObject.transform.Find("BossHealth").gameObject;
        bossHealthBarImage = bossHealthBar.transform.Find("Fill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossHealthBar.activeSelf)
        {
            bossHealthBar.SetActive(true);
        }

        bossHealthBarImage.fillAmount = hitPoints / originalHitPoints;

        if (gameObject.transform.rotation != Quaternion.identity && !bubble.activeSelf)
        {
            gameObject.transform.rotation = Quaternion.identity;
        }

        if (bubble.activeSelf)
        {
            if (!bubble.GetComponent<OwlinatorBubbleScript>().stunned)
            {
                chargeTimer -= Time.deltaTime;
                if (chargeTimer < 0 && !charging)
                {
                    targetPosition = player.transform.position;
                    charging = true;
                }

                if (charging)
                {
                    if (!returning)
                    {
                        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                        {
                            if (!bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable)
                            {
                                bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable = true;
                                currentBubbleVulnerableTimer = bubbleVulnerableTimer;
                            }
                            else
                            {
                                currentBubbleVulnerableTimer -= Time.deltaTime;
                                if (currentBubbleVulnerableTimer < 0)
                                {
                                    bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable = false;
                                    returning = true;
                                }
                            }
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, targetPosition,
                                moveSpeed / 2 * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
                        {
                            returning = false;
                            charging = false;
                            bubble.GetComponent<OwlinatorBubbleScript>().stunnable = true;
                            chargeTimer = Random.Range(chargeCooldown, chargeCooldown + 3);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, originalPosition,
                                moveSpeed / 3 * Time.deltaTime);
                        }
                    }
                }
            }
        }
        else
        {
            if (!burstShooting)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
                    moveSpeed / 8 * Time.deltaTime);
                burstShootCurrentTime -= Time.deltaTime;

                if (burstShootCurrentTime < 0)
                {
                    burstShooting = true;
                }
            }
            else
            {
                shootTimer -= Time.deltaTime;

                // if shootTimer runs out, shoot a projectile towards the player
                if (shootTimer < 0)
                {
                    GameObject bossBullet =
                        Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
                    bossBullet.GetComponent<ProjectileScript>().damage = projectileDamage;

                    bossBullet.GetComponent<Rigidbody2D>()
                        .AddForce((player.transform.position - transform.position).normalized * projectileSpeed,
                            ForceMode2D.Impulse);

                    shootTimer = shootCooldown;
                    shotCount += 1;
                    print(shotCount);
                }

                if (shotCount == 5)
                {
                    burstShooting = false;
                    burstShootCurrentTime = burstShootCooldown;
                    shotCount = 0;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            if (!collision.collider.CompareTag("Player"))
            {
                bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable = false;
                currentBubbleVulnerableTimer = bubbleVulnerableTimer;
                returning = true;
            }
        }

        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.DamagePlayer(touchDamage);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + collision.gameObject.name);
            }
        }
    }
    
    private void OnDestroy()
    {
        player.transform.Find("UI").gameObject.transform.Find("BossHealth").gameObject.SetActive(false);
        m_gameManager.completeGame();
        m_playerController.MenuActive = true;
    }
    
}