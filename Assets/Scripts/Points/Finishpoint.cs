using UnityEngine;

public class Finishpoint : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
            ActivateFinishPoint();
    }

    private void ActivateFinishPoint()
    {
        _animator.SetTrigger("Activated");
        AudioManager.Instance.PlaySFX(2);
        GameManager.Instance.LevelFinished();
    }
}
