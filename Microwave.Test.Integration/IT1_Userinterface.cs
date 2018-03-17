﻿using NUnit.Framework;
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
       
        // Stubs/mocks
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;

        // Unit under test
        private UserInterface _uut;

        [SetUp]
        public void SetUp()
        {
            // Drivers
            _driverDoor = new Door();
            _driverPowerButton = new Button();
            _driverTimeButton = new Button();
            _driverStartCancelButton = new Button();
            
            // Stubs/mocks
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();

            // Unit under test
            _uut = new UserInterface(_driverPowerButton, _driverTimeButton, _driverStartCancelButton, _driverDoor, _display, _light, _cookController);
        }

        #region Door.Integration
        [Test]
        public void OnDoorOpened_LightTurnOn_TurnOnTrue()
        {
            _driverDoor.Open();
            _light.Received().TurnOn();
        }

        [Test]
        public void OnDoorClosed_LightTurnOff_TurnOffTrue()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _light.Received().TurnOff();
        }

        #endregion

        #region Button.Integration

        [Test]
        public void OnPowerPressed_ShowPower_ShowPowerTrue()
        {
            _driverPowerButton.Press();
            _display.Received().ShowPower(50);
        }

        [Test]
        public void OnTimePressed_ShowTime_ShowTimeTrue()
        {
            // Power button skal trykkes først, for at state er SETPOWER - ELLER OGSÅ SKAL DER SKRIVES OM DER
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _display.Received().ShowTime(1,0);
        }

        [Test]
        public void OnStartCancelPressed_LightTurnOn_TurnOnTrue()
        {
            // Power og timer button skal være trykket for at være i rette state - ELLER OGSÅ SKAL DER RETTES FOR Userinterface
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _light.Received().TurnOn();
        }

        [Test]
        public void OnStartCancelPressed_CookCtrlStartCooking_StartCookingTrue()
        {
            int time = 1;
            // Power og timer button skal være trykket for at være i rette state - ELLER OGSÅ SKAL DER RETTES FOR Userinterface
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _cookController.Received().StartCooking(50,time*60);
        }

        #endregion
    }
}
