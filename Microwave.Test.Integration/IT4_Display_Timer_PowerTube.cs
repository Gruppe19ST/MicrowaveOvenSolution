using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

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
            // Der var en null-reference på output fordi output først
            // blev initialiseret efter uut sådan som det stod før. 

            // Included
            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            

            
        }

        #region Display.Integration
        [Test]
        public void StartCancelPressed_OnTimerTick_DisplayShowTime()
        {
            _driverStartCancelButton.Press();
            _cookController.StartCooking(75,1000);
            _cookController.OnTimerTick(_timer, EventArgs.Empty);
            _display.ShowTime(10,00);
            _output.Received().OutputLine("Display shows: 10:00");
        }
        #endregion

        #region Timer.Integration
        [Test]
        public void StartCancelPressed_StartCooking_TimerStart()
        {
            _driverStartCancelButton.Press();
            _cookController.StartCooking(75,1000);
            _timer.Start(1000);
        }

        [Test]
        public void StartCancelPressed_OnTimerExpired_CookingDone()
        {
            _driverStartCancelButton.Press();
            _cookController.OnTimerExpired(_timer, EventArgs.Empty);
            _userInterface.CookingIsDone();
        }

        #endregion

        #region PowerTube.Integration
        [Test]
        public void StartCancelPressed_StartCooking_PowerTubeOn_PowerTubeLogged()
        {
            _driverStartCancelButton.Press();
            _cookController.StartCooking(75,1000);
            _output.Received().OutputLine("PowerTube works with 75 %"); 
        }

        [Test]
        public void PowerTubeOff_PowerTubeLogged()
        {

        }
        #endregion



    }
}
