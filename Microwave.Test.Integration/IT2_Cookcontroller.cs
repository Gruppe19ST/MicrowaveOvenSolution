﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT2_Cookcontroller
    {
        #region Defining objects
        // Drivers (door and 3 buttons)
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;
        
        // Included
        private UserInterface _userInterface;

        // Unit under test
        private CookController _cookController;

        // Stubs/mock
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powertube;
        private ILight _light;

        #endregion

        [SetUp]
        public void Setup()
        {
            // Drivers
            _driverDoor = new Door();
            _driverPowerButton = new Button();
            _driverTimeButton = new Button();
            _driverStartCancelButton = new Button();
           
            // Stubs/mocks
            _display = Substitute.For<IDisplay>();
            _timer = Substitute.For<ITimer>();
            _powertube = Substitute.For<IPowerTube>();
            _light = Substitute.For<ILight>();

            // Unit under test
            _cookController = new CookController(_timer, _display, _powertube);

            // Included
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            
            // Property inject userinterface into cookcontroller to support the circular dependency between them
            _cookController.UI = _userInterface;
        }

        #region UserInterface.Integration
        [Test]
        public void OnStartCancelPressed_StateSetTime_PowerTubeOnTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _powertube.Received().TurnOn(1*50);
        }

        [Test]
        public void OnStartCancelPressed_StateSetTimer_StartTimer()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _timer.Received().Start(1*60);
        }

        [Test]
        public void OnTimerExpired_StateCooking_ClearDisplay()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);
            _display.Received().Clear();
        }

        #endregion
    }

}