using UnityEngine;

public class Fruit : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _animator;
    [SerializeField] private GameObject _pickupVFX;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        _gameManager = GameManager.Instance;
        SetRandomLook();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            _gameManager.AddFruit();
            AudioManager.Instance.PlaySFX(8, true);
            Destroy(gameObject);
            GameObject newFx = Instantiate(_pickupVFX, transform.position, Quaternion.identity);
        }
    }
    private void SetRandomLook()
    {
        int randomIndex = Random.Range(0, 8);
        _animator.SetFloat("FruitIndex", randomIndex);
    }
}
