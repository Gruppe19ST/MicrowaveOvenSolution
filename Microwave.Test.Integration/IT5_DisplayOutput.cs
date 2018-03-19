using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    class IT5_DisplayOutput
    {
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;

        // Stubs/mocks
        private ILight _light;
        private ITimer _timer;
        private IPowerTube _powerTube;

        // Unit under test
        private IOutput _output;

        // Included
        private UserInterface _userInterface;
        private CookController _cookController;
        private Display _display;

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
            _powerTube = Substitute.For<IPowerTube>();

            // Unit under test
            _output = Substitute.For<IOutput>();

            // Included
            _display = new Display(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_driverPowerButton,_driverTimeButton,_driverStartCancelButton,_driverDoor,_display,_light,_cookController);
            
        }

        #region Integration test

        [Test]
        public void ShowPower_LogLine_LogLineCalled()
        {
            int power = 75;
            _driverPowerButton.Press();
            _display.ShowPower(power);
            _output.Received().OutputLine($"Display shows: {power} W");
        }

        [Test]
        public void ShowTime_LogLine_LogLineCalled()
        {

        }

        [Test]
        public void Clear_LogLine_LogLineCalled()
        {
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _output.Received().OutputLine("Display cleared");
        }
        #endregion
    }
}
