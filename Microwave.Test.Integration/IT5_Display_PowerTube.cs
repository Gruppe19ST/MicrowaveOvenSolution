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
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT5_Display_PowerTube
    {
        #region Defining objects
        // Drivers (door and 3 buttons)
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Unit under test
        private Display _display;
        private PowerTube _powerTube;

        // Included
        private Timer _timer;
        private UserInterface _userInterface;
        private CookController _cookController;

        // Stubs/mocks
        private ILight _light;
        private IOutput _output;
        #endregion

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

            // Unit under test
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            // Included
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            _cookController.UI = _userInterface;


        }

        #region Display

        [Test]
        public void StartCookingForOneMinut_TimerTicksOnce_DisplayOutputIsCorrect()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            // Sleep for a bit more than a second
            Thread.Sleep(1050);
            _output.Received().OutputLine("Display shows: 00:59");
        }

        [Test]
        public void StartCookingForOneMinut_TimerExpires_DisplayOutputIsCorrect()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            // Sleep for a bit more than a minute
            Thread.Sleep(61000);
            _output.Received().OutputLine("Display cleared");
        }

        #endregion

        #region Powertube
        [Test]
        public void StartCooking_PowertubeTurnOnAt7Percent_OutputIsCorrect()
        {
            // Press Power once = 50 W = 7% of 700
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("PowerTube works with 7 %");
           
        }
        
        [Test]
        public void CookingIsDone_PowertubeTurnsOff_OutputIsCorrect()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            Thread.Sleep(61000);
            _output.Received().OutputLine("PowerTube turned off");
        }
        #endregion
    }
}

