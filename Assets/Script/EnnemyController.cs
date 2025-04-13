using UnityEngine;

namespace A1_24_25
{
    public class EnemyController : MonoBehaviour
    {
        public int id;
        private EnemyData data;

        private Rigidbody2D _rgbd2D;
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRend;

        // mvmt
        [Header("Movement")]
        [SerializeField] private float moveDistance = 2f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private bool verticalMovement = false; // deplacement horizontal/vertical

        private Vector3 _startPosition;
        private bool _movingPositiveDirection = true;

        private void Awake()
        {
            TryGetComponent(out _collider2D);
            TryGetComponent(out _rgbd2D);
            TryGetComponent(out _spriteRend);
        }

        private void Start()
        {
            data = DatabaseManager.Instance.GetData(id);
            Init();
        }

        private void Init()
        {
            name = data.label;
            float direction = Mathf.Sign(transform.localScale.x);
            transform.localScale = new Vector3(direction * data.scaleCoeff, data.scaleCoeff, 1);
            _spriteRend.sprite = data.sprite;
            _spriteRend.color = data.Color;
            _startPosition = transform.position;
        }

        public void Initialize(EnemyData newData)
        {
            data = newData;
            name = data.label;
            float direction = Mathf.Sign(transform.localScale.x);
            transform.localScale = new Vector3(direction * data.scaleCoeff, data.scaleCoeff, 1);
            _spriteRend.sprite = data.sprite;
            _spriteRend.color = data.Color;
            _startPosition = transform.position;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            float step = moveSpeed * Time.deltaTime;
            Vector3 direction = verticalMovement ? Vector3.up : Vector3.right;

            if (_movingPositiveDirection)
            {
                transform.position += direction * step;
                if ((verticalMovement && transform.position.y >= _startPosition.y + moveDistance) ||
                    (!verticalMovement && transform.position.x >= _startPosition.x + moveDistance))
                {
                    _movingPositiveDirection = false;
                    if (!verticalMovement) Flip();
                }
            }
            else
            {
                transform.position -= direction * step;
                if ((verticalMovement && transform.position.y <= _startPosition.y - moveDistance) ||
                    (!verticalMovement && transform.position.x <= _startPosition.x - moveDistance))
                {
                    _movingPositiveDirection = true;
                    if (!verticalMovement) Flip();
                }
            }
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(data.damage);
                }
            }
        }
    }
}