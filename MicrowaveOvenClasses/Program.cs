using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;

namespace MicrowaveOvenClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup all the objects, 
            var door = new Door();
            var powerButton = new Button();
            var timebutton = new Button();
            var startcancelbutton = new Button();
            var output = new Output();
            var display = new Display(output);
            var timer = new Timer();
            var powertube = new PowerTube(output);
            var cookcontroller = new CookController(timer, display, powertube);
            var light = new Light(output);

            var userinterface = new UserInterface(powerButton, timebutton, startcancelbutton, door, display, light, cookcontroller);

            cookcontroller.UI = userinterface;

            //// Simulate user activities
            //door.Open();
            //door.Close();
            //powerButton.Press();
            //timebutton.Press();
            //startcancelbutton.Press();

            //// Wait while the classes, including the timer, do their job
            //System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
            //System.Console.ReadLine();

            System.Console.WriteLine("Velkommen til din mikroovn-app #smart");
            Console.WriteLine("Indstil først power ved at trykke på 'P'.\n" +
                              "Indstil derefter tiden ved at trykke på 'T'.\n" +
                              "Når den ønskede power og tid er indstillet, start da mikroovnen ved at trykke på 'S'");

            bool running = true;

            while (running)
            {
                var tast = Console.ReadKey();
                Console.WriteLine();

                switch (tast.Key)
                {
                    case ConsoleKey.P:
                        powerButton.Press();
                        break;
                    case ConsoleKey.T:
                        timebutton.Press();
                        break;
                    case ConsoleKey.S:
                        startcancelbutton.Press();
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        break;

                }
            }


            {
                
            }
        }

    }
}
