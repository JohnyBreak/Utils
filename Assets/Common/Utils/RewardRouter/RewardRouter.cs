using System;
using System.Collections.Generic;

namespace Services.RewardRouter
{
    using RoutePoint = ValueTuple<EntityId, int>;
    using FnGeneric = Func<RoutingContext, EntityId, int, ValueTuple<EntityId, int>[]>;
    
    public class RewardRouter : IRewardRouter
    {
        private readonly Dictionary<EntityTypeCode, FnGeneric> _collectors = new();
        private readonly Dictionary<EntityTypeCode, FnGeneric> _emitters = new();
        
        public void Route(RoutingContext ctx, (EntityId entity, int amount) collect, (EntityId entity, int amount) emit)
        {
            Collect(ctx, collect);
            Emit(ctx, emit);
        }

        public void RegisterCollector(EntityTypeCode entityTypeCode, FnGeneric collector)
        {
            _collectors[entityTypeCode] = collector;
        }

        public void RegisterEmitter(EntityTypeCode entityTypeCode, FnGeneric emitter)
        {
            _emitters[entityTypeCode] = emitter;
        }
        
        private void Collect(RoutingContext ctx, RoutePoint collect)
        {
            var processed = CollectInternal(ctx, collect.Item1, collect.Item2);

            if (processed == null || processed.Length < 1)
            {
                return;
            }

            foreach (var point in processed)
            {
                Collect(ctx, point);
            }
        }

        private void Emit(RoutingContext ctx, RoutePoint emit)
        {
            var processed = EmitInternal(ctx, emit.Item1, emit.Item2);
        
            if (processed == null || processed.Length < 1)
            {
                return;
            }

            foreach (var point in processed)
            {
                Emit(ctx, point);
            }
        }
        
        private RoutePoint[]? CollectInternal(RoutingContext ctx, EntityId entity, int amount)
        {
            if (!_collectors.TryGetValue(entity.TypeCode, out var collector))
            {
                throw new InvalidOperationException($"No registered collectors for '{entity.TypeCode}'");
            }

            return collector.Invoke(ctx, entity, amount);
        }

        private RoutePoint[]? EmitInternal(RoutingContext ctx, EntityId entity, int amount)
        {
            if (!_emitters.TryGetValue(entity.TypeCode, out var emitter))
            {
                throw new InvalidOperationException($"No registered emitters for '{entity.TypeCode}'");
            }

            return emitter.Invoke(ctx, entity, amount);
        }
    }
}