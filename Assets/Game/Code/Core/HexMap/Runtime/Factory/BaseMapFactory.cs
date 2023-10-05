using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public abstract class BaseMapFactory : ScriptableObject, IHexMapFactory
    {
        public abstract IHexMap Create();
    }
}