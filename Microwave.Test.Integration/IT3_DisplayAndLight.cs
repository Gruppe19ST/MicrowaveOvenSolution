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
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Stubs/mocks
        private ICookController _cookController;
        private IOutput _output;

        // Unit under test
        private IDisplay _display;
        private ILight _light;

        // Included
        private UserInterface _userInterface;

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

            // Unit under test
            _display = new Display(_output);
            _light = new Light(_output);

            // Included
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            
        }

        #region Display

        [Test]
        public void PowerPressedOnce_ShowPower_PowerLogged()
        {
            _driverPowerButton.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void PowerPressedTwice_ShowPower_PowerLogged()
        {
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _output.Received().OutputLine("Display shows: 100 W");
        }

        [Test]
        public void TimePressedOnce_ShowTime_TimeLogged()
        {
            _driverTimeButton.Press();
            _output.Received().OutputLine("Display shows: 01:00");
        }
        [Test]
        public void TimePressedTwice_ShowTime_TimeLogged()
        {
            _driverTimeButton.Press();
            _driverTimeButton.Press();
            _output.Received().OutputLine("Display shows: 02:00");

        }


        #endregion

        #region Light
        [Test]
        public void LightOn_ShowLightOn_LightOnlogged()
        {
            _driverDoor.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void StartCancelBtn_ShowLightOn_LightOnlogged()
        {
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void LightOff_ShowLightOff_LightOfflogged()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _output.Received().OutputLine("Light is turned off");
        }

        // Test af light off når cooking is done? 



        #endregion
    }
}
