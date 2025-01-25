using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public enum ItemType { WEAPON_GUN, WEAPON_SPREAD, WEAPON_LANDMINE }
    public ItemType itemType;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (itemType == ItemType.WEAPON_GUN)
            {
                other.gameObject.GetComponent<MainCharacterController>().weaponGun.isActive = true;
                Destroy(gameObject);
            }
            else if (itemType == ItemType.WEAPON_SPREAD)
            {
                other.gameObject.GetComponent<MainCharacterController>().weaponSpread.isActive = true;
                Destroy(gameObject);
            }
            
            else if (itemType == ItemType.WEAPON_LANDMINE)
            {
                other.gameObject.GetComponent<MainCharacterController>().weaponLandmine.isActive = true;
                Destroy(gameObject);
            }
        }
    }
}
