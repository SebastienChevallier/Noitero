using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int Health { get; set; }

    public int MaxHealth { get;}

    public void Damage(int damage)
    {
        Health -= damage;

        if(Health < 0)
        {
            Health = MaxHealth; //Testing
            //Death
        }
    }
}
