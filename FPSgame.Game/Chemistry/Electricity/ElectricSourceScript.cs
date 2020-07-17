using FPSgame.Core;
using Stride.Engine;
using Stride.Engine.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;

namespace FPSgame.Chemistry.Electricity
{
    class ElectricSourceScript : AsyncScript
    {
        private ElectricCurrentGraph graph;
        private EventReceiver rebuildGraph = new EventReceiver(ElectricConductionScript.currentChange);

        public override async Task Execute()
        {
            graph = new ElectricCurrentGraph(Entity);
            Entity.Add(new ElectricConductionScript());
            while (Game.IsRunning)
            {
                await rebuildGraph.ReceiveAsync();
                graph = new ElectricCurrentGraph(Entity);
            }
            
        }
    }
}
