﻿using System;

namespace OOPIntro
{
    class Program
    {
        static void Main(string[] args)
        {
         
        Vehicle TeslaRoadster = new Vehicle();
        TeslaRoadster.Move(770);
        System.Console.WriteLine("Num of Passengers in Roadster: " + TeslaRoadster.numPassenger);
        System.Console.WriteLine("Distance driven in Roadster: " + TeslaRoadster.distance);

        Vehicle myBike = new Vehicle();
        myBike.Move(1.3);
        System.Console.WriteLine("Num of Passengers on Bike: " + myBike.numPassenger);
        System.Console.WriteLine("Distance driven on Bike: " + myBike.distance);

        }
    }
}
