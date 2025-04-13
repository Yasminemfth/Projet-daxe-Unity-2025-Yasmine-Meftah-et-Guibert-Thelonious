using UnityEngine;

[System.Serializable]

//struct pour gerer les stats du joueur
public struct PlayerStats
{
    public float speed;
    public int life;

    public PlayerStats(float speed, int life)
    {
        this.speed = speed;
        this.life = life;
    }
}
