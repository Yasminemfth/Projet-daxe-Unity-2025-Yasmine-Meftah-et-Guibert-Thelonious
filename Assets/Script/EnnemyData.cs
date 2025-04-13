using System.Collections.Generic;
using UnityEngine;

namespace A1_24_25
{
    [System.Serializable]
    public class EnemyData
    {
        public string label;

        [Header("SETUP")]
        [Range(0.1f, 3f)]
        public float scaleCoeff;

        public Sprite sprite;

        [ColorUsage(true, true)]
        public Color baseColor;

        public Color Color => new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        [Header("STATS")]
        [Min(1)] public int pv;
        [Min(0f)] public float speed;
        [Min(0)] public int damage;

        // Constructeur 
        public EnemyData(string label, float scaleCoeff, Sprite sprite, Color baseColor, int pv, float speed, int damage)
        {
            this.label = label;
            this.scaleCoeff = scaleCoeff;
            this.sprite = sprite;
            this.baseColor = baseColor;
            this.pv = pv;
            this.speed = speed;
            this.damage = damage;
        }
    }
}