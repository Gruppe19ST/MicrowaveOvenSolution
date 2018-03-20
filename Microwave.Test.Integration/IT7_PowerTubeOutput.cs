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
    class IT7_PowerTubeOutput
    {
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Stubs/mocks
        private ILight _light;
        private ITimer _timer;
        private IDisplay _display;

        // Unit under test
        private Output _output;

        // Included
        private UserInterface _userInterface;
        private CookController _cookController;
        private PowerTube _powerTube;

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
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();

            // Unit under test
            _output = new Output();

            // Included
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            _cookController.UI = _userInterface;

        }

        #region PowerTube-Output.Integration

        [Test]
        public void OnStartCancelPressed_PowerTubeTurnOn_LogLineCalled()
        {
            _driverPowerButton.Press();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.OutputLine($"PowerTube works with {100} %");
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
