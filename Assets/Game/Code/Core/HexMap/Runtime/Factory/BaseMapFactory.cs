using UnityEngine;

namespace YellowSquad.Core.HexMap
{
    public abstract class BaseMapFactory : ScriptableObject, IHexMapFactory
    {
        public abstract IHexMap Create();
    }
}