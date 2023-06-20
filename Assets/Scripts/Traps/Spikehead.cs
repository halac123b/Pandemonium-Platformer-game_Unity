using UnityEngine;

public class Spikehead : EnemyDamage
{
  [Header("SpikeHead Attributes")]
  [SerializeField] private float speed;   // Move speed
  [SerializeField] private float range; // Attack range
  [SerializeField] private float checkDelay;  // Khoảng delay sau mỗi lần check player có trong tầm hay không
  [SerializeField] private LayerMask playerLayer;
  private Vector3[] directions = new Vector3[4];    // 4 vector tương ứng check player theo 4 hướng
  private Vector3 destination;  // Đích đến để attack đã đc xác định
  private float checkTimer;   // Timer
  private bool attacking;   // Flag spikehead có đang di chuyển tấn công hay không

  [Header("SFX")]
  [SerializeField] private AudioClip impactSound;

  private void OnEnable()
  {
    Stop();
  }
  private void Update()
  {
    //Move spikehead to destination only if attacking
    if (attacking)
      transform.Translate(destination * Time.deltaTime * speed);
    else
    {
      checkTimer += Time.deltaTime;
      if (checkTimer > checkDelay)  // Cứ mỗi 1 khoảng delay thì check player 1 lần
        CheckForPlayer();
    }
  }
  private void CheckForPlayer()
  {
    CalculateDirections();

    //Check if spikehead sees player in all 4 directions
    for (int i = 0; i < directions.Length; i++)
    {
      Debug.DrawRay(transform.position, directions[i], Color.red);
      RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

      if (hit.collider != null && !attacking)
      {
        attacking = true;
        destination = directions[i];
        checkTimer = 0;
      }
    }
  }
  private void CalculateDirections()
  {
    directions[0] = transform.right * range; //Right direction
    directions[1] = -transform.right * range; //Left direction
    directions[2] = transform.up * range; //Up direction
    directions[3] = -transform.up * range; //Down direction
  }
  private void Stop()
  {
    destination = transform.position; //Set destination as current position so it doesn't move
    attacking = false;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    SoundManager.instance.PlaySound(impactSound);
    base.OnTriggerEnter2D(collision);
    Stop(); //Stop spikehead once he hits something
  }
}