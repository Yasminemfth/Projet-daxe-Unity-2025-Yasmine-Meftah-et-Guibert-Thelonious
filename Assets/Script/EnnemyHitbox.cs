using UnityEngine;

//hitbox de l'ennemi pour quand joueur touche l'ennemi il perd de la vie (le joueur)
public class EnemyHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(10); 
            }
        }
    }
}
