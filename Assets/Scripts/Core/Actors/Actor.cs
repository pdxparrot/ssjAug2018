namespace pdxpartyparrot.Core.Actors
{
    public interface IActor
    {
        int Id { get; }

        ActorController Controller { get; }

        void Initialize(int id);
    }
}
