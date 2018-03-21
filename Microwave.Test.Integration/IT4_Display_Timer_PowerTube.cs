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
    public class IT4_Display_Timer_PowerTube
    {
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;
        // Unit under test
        private Display _display;
        private Timer _timer;
        private PowerTube _powerTube;
        // Included
        private UserInterface _userInterface;
        private CookController _cookController;
        // Stubs/mocks
        private ILight _light;
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

            // Unit under test
            _display = new Display(_output);
            _timer = new Timer();
            _powerTube = new PowerTube(_output);

            // Included
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            _cookController.UI = _userInterface;


        }

        #region DisplayAndTimer

        [Test]
        public void StartCookingForOneMinut_TimerStarts_DisplayOutputIsCorrect()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();

            Thread.Sleep(1050);
            _output.Received().OutputLine("Display shows: 00:59");

        }
        [Test]
        public void StartCookingForTwoMinut_TimerStarts_DisplayOutputIsCorrect()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();

            Thread.Sleep(1050);
            _output.Received().OutputLine("Display shows: 01:59");

        }

        #endregion

        #region PowertubeAndTimer

        [Test]
        public void StartCooking_PowertubeAt50_OutputIsCorrect()
        {
            // Press Power 7 times to hit 350 W = 50%
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("PowerTube works with 50 %");
           
        }
        [Test]
        public void StartCooking_PowertubeAt100_OutputIsCorrect()
        {
            // Press Power 14 times to hit 700 W = 100%
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("PowerTube works with 100 %");
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

    [TestFixture]
    public class IT4_Circular_Display_Timer
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
        public void OnTimerTick_TimePressedTwice_ShowTimeTrue()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            // Sleep at least two seconds
            Thread.Sleep(2250);
            _display.Received().ShowTime(1, 58);
        }
    }
}

