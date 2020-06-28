using System;
using System.Collections.Generic;
using Stride.Engine;
using Stride.Physics;
using System.Linq;
using FPSgame.Chemistry;

namespace FPSgame.Core
{
    
    public class ElectricCurrentGraph
    {
        public Entity Root {get;set;}
        public Dictionary<Entity,List<Entity>> AdjacencyLists{get;set;}

        public ElectricCurrentGraph(Entity root)
        {
            Root = root;
            // var rb = Root.Get<RigidbodyComponent>();
            var electities = 
                root
                    .Scene
                    .Entities
                    .AsParallel()
                    .Where(e => e.Get<ElectricConductionScript>() != null && e.Get<ElectricConductionScript>().Conduct == Current.ON);
        }
        public void FillAdjacencyLists(Entity root, Entity parent)
        {
            var collisions = root.Get<RigidbodyComponent>()?.Collisions;
            var parentRb = parent?.Get<RigidbodyComponent>();
            if(collisions?.Count == 0)
                return;
            else if(collisions?.Count == 1 && (collisions?.AsEnumerable().First()?.ColliderB == parentRb))
                return;
            else
            {

            }
            return;
        }
    }
}