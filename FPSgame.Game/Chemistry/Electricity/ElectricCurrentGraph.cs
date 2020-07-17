using System;
using System.Collections.Generic;
using Stride.Engine;
using Stride.Physics;
using System.Linq;
using FPSgame.Chemistry;

namespace FPSgame.Chemistry.Electricity
{
    public class ElectricCurrentGraph
    {
        private Entity Root {get;set;}
        private Dictionary<Entity, List<Entity>> AdjacencyLists { get; set; } = new Dictionary<Entity, List<Entity>>();

        public ElectricCurrentGraph(Entity root)
        {
            Root = root;
            root.Scene.Entities
                .AsParallel()
                .Where(e => e.Get<ElectricConductionScript>() != null && e.Get<ElectricSourceScript>() == null)
                .ForAll(e => e.Get<ElectricConductionScript>().Conduct = Current.OFF);
            FillAdjacencyLists(root);
        }
        public void FillAdjacencyLists(Entity root)
        {
            if (root == null)
                return;

            // List all conducive connected bodies
            var colliders =
                root.Get<RigidbodyComponent>()
                    .Collisions
                    .AsParallel()
                    .Where(c => (c.ColliderA as RigidbodyComponent) != null && (c.ColliderB as RigidbodyComponent != null))
                    .Select(c => (c.ColliderA as RigidbodyComponent).Equals(root.Get<RigidbodyComponent>()) ? c.ColliderB as RigidbodyComponent : c.ColliderA as RigidbodyComponent)
                    .Where(x => x != null);
            List<Entity> connectedEntities = 
                    colliders
                    .Select(rb => 
                        root.Scene.Entities.FirstOrDefault(
                            e => e.Get<RigidbodyComponent>() == rb && !root.Get<ElectricConductionScript>().Equals(null)))
                    .ToList();
            AdjacencyLists.Add(root, connectedEntities);

            // Activate electric current
             connectedEntities
                .AsParallel()
                .Where(e => e.Get<ElectricConductionScript>() != null)
                .ForAll(e => e.Get<ElectricConductionScript>().Conduct = Current.ON);

            foreach (var e in connectedEntities)
            {
                if(!AdjacencyLists.ContainsKey(e))
                    FillAdjacencyLists(e);
            }
        }
    }
}