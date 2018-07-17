using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorManager<T> : SingletonBehavior<ActorManager<T>> where T: IActor
    {
    }
}
