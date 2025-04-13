using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical; 
    private float speed = 8f; 
    private bool isLadder;     private bool isClimbing; 

    [SerializeField] private Rigidbody2D rb; 

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical"); 

        // verifie si on est sur l'échelle et qu'on appuie en haut ou en bas
        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true; // grimpe
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f; // en grimpant : pas de gravité
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * speed); 
        }
        else
        {
            rb.gravityScale = 4f; // gravité normale
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false; //  quitte l'échelle
            isClimbing = false; // arrête de monter/grimper
        }
    }
}