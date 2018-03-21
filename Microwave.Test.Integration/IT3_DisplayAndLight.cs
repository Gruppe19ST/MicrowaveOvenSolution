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
    class IT3_DisplayAndLight
    {
        #region Defining objects
        // Drivers (door and 3 buttons)
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Stubs/mocks
        private ICookController _cookController;
        private IOutput _output;

        // Units under test
        private IDisplay _display;
        private ILight _light;

        // Included
        private UserInterface _userInterface;
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
            _output = Substitute.For<IOutput>();
            _cookController = Substitute.For<ICookController>();

            // Units under test
            _display = new Display(_output);
            _light = new Light(_output);

            // Included
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            
        }

        #region Light
        [Test] // UC pkt.2
        public void OpenDoor_TurnLightOn_LightOnlogged()
        {
            _driverDoor.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test] // UC pkt.5 
        public void CloseDoor_TurnLightOff_LightOfflogged()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _output.Received().OutputLine("Light is turned off");
        }

        [Test] // UC pkt.9
        public void StartCancelBtnPressed_TurnLightOn_LightOnlogged()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("Light is turned on");
        }

        #endregion

        #region Display
        [Test] // UC pkt.6
        public void PowerPressedOnce_ShowPower_PowerLogged()
        {
            _driverPowerButton.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test] // UC pkt.6
        public void PowerPressedTwice_ShowPower_PowerLogged()
        {
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _output.Received().OutputLine("Display shows: 100 W");

        }

        [Test] // UC pkt.7
        public void TimePressedOnce_ShowTime_TimeLogged()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _output.Received().OutputLine("Display shows: 01:00");
        }
        [Test] // UC pkt.7
        public void TimePressedTwice_ShowTime_TimeLogged()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverTimeButton.Press();
            _output.Received().OutputLine("Display shows: 02:00");

        }

        #endregion

        #region Extensions
        [Test] // Extension 3
        public void StartCancelBtnWhileCooking_TurnLightOff_LightOfflogged()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("Light is turned off");
        }

        #endregion




    }
}
