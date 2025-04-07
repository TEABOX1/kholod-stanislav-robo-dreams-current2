using System;
using UnityEngine;

namespace AllInOne
{
    public abstract class ItemBase : ScriptableObject, IItem
    {
        [SerializeField] protected string id;
        [SerializeField] protected int maxStack;
        [SerializeField] protected string name;
        [SerializeField, TextArea] protected string description;
        
        public string Id => id;
        public abstract Type ItemType { get; }
        public int MaxStack => maxStack;
        public string Name => name;
        public string Description => description;
    }
}