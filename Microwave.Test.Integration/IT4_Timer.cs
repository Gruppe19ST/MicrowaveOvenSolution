﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT4_Timer
    {
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Unit under test
        private Timer _timer;

        // Included
        private UserInterface _userInterface;
        private CookController _cookController;

        // Stubs/mocks
        private IDisplay _display;
        private ILight _light;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            // Drivers
            _driverDoor = new Door();
            _driverPowerButton = new Button();
            _driverTimeButton = new Button();
            _driverStartCancelButton = new Button();

            // Stubs/mocks
            _light = Substitute.For<ILight>();
            _output = Substitute.For<IOutput>();
            _powerTube = Substitute.For<IPowerTube>();
            _display = Substitute.For<IDisplay>();

            // Unit under test
            _timer = new Timer();

            // Der var en null-reference på output fordi output først
            // blev initialiseret efter uut sådan som det stod før. 

            // Included
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            _cookController.UI = _userInterface;


        }

        // Test that when the timer ticks, cookcontroller tells display to show remaining time
        [Test]
        public void OnTimerTick_TimePressedOnce_ShowTimeTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            // Sleep at least a second
            Thread.Sleep(1250);
            _display.Received().ShowTime(0, 59);
        }

        [Test]
        public void OnTimerExpired_TimePressedOnce_ShowTimeTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            // Sleep at least a minute
            Thread.Sleep(61000);
            _powerTube.Received().TurnOff();
        }
    }
    
}
