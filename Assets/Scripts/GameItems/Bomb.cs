using Assets.Scripts.Helpers;
using System;
using System.Linq;
using UnityEngine;


public class Bomb : GameItem
{
    public ParticleSystem ParticleSystem;
    public float Timer;
    public float Radius;
    public float Damage;

    private bool exploded;

    public void SetUp(float timer, float radius, float damage)
    {
        Timer = timer;
        Radius = radius;
        Damage = damage;
    }
    private void Update()
    {
        if (Timer <= 0)
        {
            Explode();
        }

        Timer -= Time.deltaTime;
    }

    private void Explode()
    {
        if (!exploded)
        {
            exploded = true;
            myCollider.enabled = false;
            AffectNeighbour(Vector3.forward, Damage, Radius);
            AffectNeighbour(Vector3.back, Damage, Radius);
            AffectNeighbour(Vector3.left, Damage, Radius);
            AffectNeighbour(Vector3.right, Damage, Radius);
            Destroy(gameObject, 0.25f);
        }
    }

    private void AffectNeighbour(Vector3 offset, float damage, float rangeLeft)
    {
        var explosionPosition = transform.position + offset;
        var gameItems = SpaceChecker.GetCollidingGameItems(explosionPosition, gameObject);
        foreach (var gameItem in gameItems)
        {
            gameItem.OnHit();
        }

        if (rangeLeft-- == 0 || gameItems.Any(gi => gi.BlocksExplosion))
        {
            return;
        }
        ShowParticles(offset);
        AffectNeighbour(offset + offset.normalized, damage, rangeLeft);
    }

    private void ShowParticles(Vector3 position)
    {
        var emitParams = new ParticleSystem.EmitParams()
        {
            position = position
        };
        ParticleSystem.Emit(emitParams, 1);
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<BoxCollider>().isTrigger = false;
    }

    public override void OnHit()
    {
        Explode();
    }
}
