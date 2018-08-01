using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorManager<T> : SingletonBehavior<ActorManager<T>> where T: IActor
    {
        protected List<T> Actors { get; } = new List<T>();

        public void Register(T actor)
        {
            Actors.Add(actor);
        }

        public void Unregister(T actor)
        {
            Actors.Remove(actor);
        }
    }
}
