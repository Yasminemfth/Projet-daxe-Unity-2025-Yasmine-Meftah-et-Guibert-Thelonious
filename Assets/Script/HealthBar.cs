using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Références UI")]
    public Slider slider;

//gere la vie du joueur
    public int CurrentValue
    {
        get => (int)slider.value;
        set
        {
            if (slider == null)
            {
                Debug.LogWarning("Le Slider n'est pas assigné !");
                return;
            }
            slider.value = Mathf.Clamp(value, 0, slider.maxValue);
        }
    }


    /// Définit la vie maximale 
    
    public void SetMaxHealth(int health)
    {
        if (slider == null)
        {
            Debug.LogWarning("Le Slider n'est pas assigné dans l'inspecteur !");
            return;
        }

        slider.maxValue = health;
        CurrentValue = health; // utilise le set
    }

    /// maj la vie sur la barre si il se fait toucher 
    
    public void SetHealth(int health)
    {
        CurrentValue = health; // utilise le set aussi ici
    }
}
