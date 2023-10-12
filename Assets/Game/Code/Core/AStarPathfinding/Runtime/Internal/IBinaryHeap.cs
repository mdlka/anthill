namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    internal interface IBinaryHeap<in TKey, T>
    {
        int Count { get; }

        void Enqueue(T item);
        T Dequeue();
        void Clear();

        void Modify(T value);
        bool TryGet(TKey key, out T value);
    }
}
