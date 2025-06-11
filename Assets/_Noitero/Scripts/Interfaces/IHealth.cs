using UnityEngine;

public interface IHealth
{
    public int MaxHealth { get;}
    public int Health { get; set; }
    public void Damage(int damage);
}
