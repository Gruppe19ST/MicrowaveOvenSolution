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
        #region Defining objects
        // Drivers (door and 3 buttons)
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
        public void OnPowerPressed_PowerPressedOnce_ShowPowerTrue()
        {
            _driverPowerButton.Press();
            _display.Received().ShowPower(1*50);
        }

        [Test]
        public void OnTimePressed_TimePressedOnce_ShowTimeTrue()
        {
            // Must push PowerButton before being able to push TimeButton
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _display.Received().ShowTime(1,0);
        }

        [Test]
        public void OnStartCancelPressed_StateSetTime_TurnOnLight()
        {
            // Must push PowerButton before being able to push TimeButton
            _driverPowerButton.Press();
            // Must push TimeButton to be in state "SetTime"
            _driverTimeButton.Press();
            // Push Start/CancelButton to start microoven
            _driverStartCancelButton.Press();
            _light.Received().TurnOn();
        }

        [Test]
        public void OnStartCancelPressed_StateSetTime_StartCookingCookCtrl()
        {
            // Must push PowerButton before being able to push TimeButton
            _driverPowerButton.Press();
            // Must push TimeButton to be in state "SetTime"
            _driverTimeButton.Press();
            // Push Start/CancelButton to start microoven
            _driverStartCancelButton.Press();
            _cookController.Received().StartCooking(50,1*60);
        }

        #endregion

        #region Extentions

        #region Door.Extensions
        [Test] // Extention 1: User presses StartCancel btn during power setup
        public void StartCancelDuringSetup_ClearDisplay_DisplayIsCleared()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _driverPowerButton.Press();
            _driverStartCancelButton.Press();
            _display.Received().Clear();
        }

        [Test] // Extention 3: User presses StartCancel btn during cooking
        public void StartCancelDuringCooking_CookingStops_CookingStopped()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _driverStartCancelButton.Press();
            _display.Received().Clear();
            _light.Received().TurnOff();
            _cookController.Received().Stop();

        }
        #endregion

        #region Button.Extensions
        [Test] // Extention 2_1: User opens door during power setup
        public void DoorOpenDuringPowerSetup_ClearDispAndLightOn_DisplayIsClearedAndLightIsOn()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _driverPowerButton.Press();
            _driverDoor.Open();
            _light.Received().TurnOn();
            _display.Received().Clear();
        }

        [Test] // Extention 2_2: User opens door during time setup
        public void DoorOpenDuringTimeSetup_ClearDispAndLightOn_DisplayIsClearedAndLightIsOn()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverDoor.Open();
            _light.Received().TurnOn();
            _display.Received().Clear();
        }
        
        [Test] // Extention 4: User opens door during cooking
        public void DoorOpensDuringCooking_CookingStops_CookingStopped()
        {
            _driverDoor.Open();
            _driverDoor.Close();
            _driverPowerButton.Press();
            _driverTimeButton.Press();
            _driverStartCancelButton.Press();
            _driverDoor.Open();
            _display.Received().Clear();
            _light.Received().TurnOff();
            _cookController.Received().Stop();

        }
        #endregion

        #endregion
    }
}
