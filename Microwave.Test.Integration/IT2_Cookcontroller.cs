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
        private ICookController _cookController;
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

            // Included
            _userInterface = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
            // Unit under test
            _cookController = new CookController(_timer, _display, _powertube);
        }

        #region UserInterface.Integration
        [Test]

        public void OnStartCancelPressed_StartCookingTrue()
        {
            _driverStartCancelButton.Press();
            _cookController.StartCooking(75,1000);
            _light.Received().TurnOn();


        }

        #endregion
    }

}