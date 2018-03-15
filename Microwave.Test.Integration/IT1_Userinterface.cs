using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_Userinterface
    {
        // Drivers
        private Door _driverDoor;
        private Button _driverPowerButton;
        private Button _driverTimeButton;
        private Button _driverStartCancelButton;
        // Unit under test
        private UserInterface _uut;
        // Stubs/mocks
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;

        [SetUp]
        public void Setup()
        {
            // Drivers
            _driverDoor = new Door();
            _driverPowerButton = new Button();
            _driverTimeButton = new Button();
            _driverStartCancelButton = new Button();
            // Unit under test
            _uut = new UserInterface(_driverPowerButton,_driverTimeButton,_driverStartCancelButton,_driverDoor,_display,_light,_cookController);
            // Stubs/mocks
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
        }
    }
}
