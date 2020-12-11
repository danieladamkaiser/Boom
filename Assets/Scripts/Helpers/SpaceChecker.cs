using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class SpaceChecker
    {
        public static IEnumerable<GameItem> GetCollidingGameItems(Vector3 position, GameObject self)
        {
            var gameItems = Physics.OverlapSphere(position, 0.4f)
                .Select(i => i.GetComponent<GameItem>())
                .Where(gi => gi != null && gi.gameObject != self);

            return gameItems;
        }
    }
}
