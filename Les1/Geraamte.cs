using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geraamte : MonoBehaviour {

}

public class ExplosiveBarrel : MonoBehaviour, IDamager {
    protected void Die(){

    }
    public DamageInfo damageInfo {
        get;
    }

}

public class Gun : MonoBehaviour {
    public void Fire(){

    }
}

public class Entity : MonoBehaviour, IDamagable {
    protected int health(){
        return 0;
    }

    protected void Die(){

    }
    public void onDeath() {

    }
    public bool TakeDamage(IDamager damager)
    {
        return true;
    }
}

public class DamageInfo : MonoBehaviour {
    public int amount(){
        return 0;
    }
}

public class Player : MonoBehaviour {
    public Gun gun(){
        return null;
    }

    protected void HandleWeapons(){

    }
}

public interface IDamager {
    DamageInfo damageInfo { get; }
}

public interface IDamagable{
    void onDeath();
    bool TakeDamage(IDamager damager);
    
}