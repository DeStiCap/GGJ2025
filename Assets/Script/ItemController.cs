using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public enum ItemType { WEAPON_GUN, WEAPON_SPREAD, WEAPON_LANDMINE, ITEM_PROTECT_STUN, ITEM_ATK_BOOST, ITEM_DEF_DEBUFF }
    public ItemType itemType;
    public AudioClip collectSoundEffect;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(collectSoundEffect, transform.position);
            
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
            
            else if (itemType == ItemType.ITEM_PROTECT_STUN)
            {
                other.gameObject.GetComponent<MainCharacterController>().buffDebuff.isProectedFromStun = true;
                other.gameObject.GetComponent<MainCharacterController>().buffDebuff.protectedStunTime = Time.time;
                Destroy(gameObject);
            }
            
            else if (itemType == ItemType.ITEM_ATK_BOOST)
            {
                other.gameObject.GetComponent<MainCharacterController>().buffDebuff.isAtkBoosted = true;
                other.gameObject.GetComponent<MainCharacterController>().buffDebuff.lastAtkBoosted = Time.time;
                Destroy(gameObject);
            }
            
            else if (itemType == ItemType.ITEM_DEF_DEBUFF)
            {
                other.gameObject.GetComponent<MainCharacterController>().buffDebuff.isDefDebuff = true;
                other.gameObject.GetComponent<MainCharacterController>().buffDebuff.lastDefDebuff = Time.time;
                Destroy(gameObject);
            }
        }
    }
}
