using UnityEngine;

public interface IFireable
{
    public void InitialiseAmmo(
        AmmoDetailsSO ammoDetails, float ammoSpeed, float ammoRange, Vector2 directionVector, bool isAmmoSet, bool isFieldEffect);

    public GameObject GetGameObject();
}
