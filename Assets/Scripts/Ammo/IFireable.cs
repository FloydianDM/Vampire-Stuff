using UnityEngine;

public interface IFireable
{
    public void InitialiseAmmo(int ammoDamage, float ammoSpeed, float ammoRange, Vector2 directionVector, bool isAmmoSet, 
        bool isFieldEffect);

    public GameObject GetGameObject();
}
