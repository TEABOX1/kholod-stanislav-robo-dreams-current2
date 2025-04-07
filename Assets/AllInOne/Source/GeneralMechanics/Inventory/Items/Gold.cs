using System;
using UnityEngine;

namespace AllInOne
{
    [CreateAssetMenu(fileName = "Gold", menuName = "Data/Items/Gold", order = 0)]
    public class Gold : ItemBase
    {
        public override Type ItemType { get; } = typeof(Gold);
    }
}