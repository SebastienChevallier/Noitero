using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int Health { get; set; }

    public int MaxHealth { get;}
    private int totalDamageTaken = 0;

    public UnityEvent<string> events;
    

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health < 0)
            Health = MaxHealth; // TODO: death handling
        totalDamageTaken += damage;
        events.Invoke(totalDamageTaken.ToString());
    }
}
