using System;
using UnityEngine;

namespace AllInOne
{
    [CreateAssetMenu(fileName = "Currency", menuName = "Data/Items/Currency", order = 0)]
    public class Currency : ItemBase
    {
        public override Type ItemType { get; } = typeof(Currency);
    }
}