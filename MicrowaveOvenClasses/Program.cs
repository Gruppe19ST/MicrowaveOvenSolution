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

            // Simulate user activities
            door.Open();
            door.Close();
            powerButton.Press();
            timebutton.Press();
            startcancelbutton.Press();

            // Wait while the classes, including the timer, do their job
            System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
            System.Console.ReadLine();
        }

    }
}
