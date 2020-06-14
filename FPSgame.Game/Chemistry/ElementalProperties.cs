using System;

namespace FPSgame.Chemistry 
{
    public enum ElementType
    {
        Metal,
        Wood,
        Water,
        Mineral
    }
    public enum Conductivity 
    {
        Conductor,
        Insulator
    }
    public class ThermalSimulation
    {
        public static float timestep = 1;
    }
}