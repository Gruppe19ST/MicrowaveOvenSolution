using System;
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
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;
        //LEG
        // Included
        private UserInterface _userInterface;
        // Unit under test
        private CookController _cookController;
        //stubs/mock
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powertube;
        private ILight _light;

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

            _cookController.UI = _userInterface;
        }

        #region UserInterface.Integration
        [Test]
        public void OnStartCancelPressed_PowerPressedOnce_PowerTubeOnTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _powertube.Received().TurnOn(1*50);
        }

        [Test]
        public void OnStartCancelPressed_PowerPressedTwice_PowerTubeOnTrue()
        {
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _powertube.Received().TurnOn(2*50);
        }

        [Test]
        public void OnStartCancelPressed_TimerPressedOnce_TimerOnTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _timer.Received().Start(1*60);
        }

        [Test]
        public void OnStartCancelPressed_TimerPressedTwice_TimerOnTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _timer.Received().Start(2*60);
        }

        [Test]
        public void OnTimerTick_TimerPressedOnce_DisplayShowTrue()
        {
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            //Thread.Sleep(1850);
            //_display.Received().ShowTime(01,59);
            //_display.Received().ShowPower(100);
        }

        [Test]
        public void OnTimerExpired_TimerPressedOnce_DisplayClearTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _display.Received().Clear();

        }
        #endregion
    }

}