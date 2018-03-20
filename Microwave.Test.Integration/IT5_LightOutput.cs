using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    class IT5_LightOutput
    {
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Stubs/mocks
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;

        // Unit under test
        private Output _output;

        // Included
        private UserInterface _userInterface;
        private CookController _cookController;
        private Light _light;

        [SetUp]
        public void SetUp()
        {
            // Drivers
            _driverDoor = new Door();
            _driverPowerButton = new Button();
            _driverTimeButton = new Button();
            _driverStartCancelButton = new Button();

            // Stubs/mocks
            _display = Substitute.For<IDisplay>();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();

            // Unit under test
            _output = new Output();

            // Included
            _light = new Light(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton,
                _driverDoor, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        #region Light-Output.Integration

        [Test]
        public void OpenDoor_LightTurnOn_LogLineCalled()
        {
            _driverDoor.Open();
            _output.OutputLine("Light is turned on");
        }

        [Test]
        public void OpenDoor_LightTurnOff_LogLineCalled()
        {
            _driverDoor.Close();
            _output.OutputLine("Light is turned off");
        }

        [Test]
        public void CookingIsDone_LightTurnOff_LogLineCalled()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();

            _output.OutputLine("Light is turned off");
        }

        #endregion
    }
}
