using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class TD_IT1_Door_UI
    {
        private ICookController _fakeCoockController;
        private IDisplay _fakeDisplay;
        private ILight _fakeLight;
        private Door _sut;
        private IUserInterface _userInterface;
        private IButton fakePowerButton;
        private IButton fakeTimeButton;
        private IButton fakeStartCancelButton;

        [SetUp]
        public void SetUp()
        {
            fakePowerButton = Substitute.For<IButton>();
            fakeTimeButton = Substitute.For<IButton>();
            fakeStartCancelButton = Substitute.For<IButton>();
            _fakeCoockController = Substitute.For<ICookController>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();

            _sut = new Door();


            _userInterface = new UserInterface(fakePowerButton, fakeTimeButton, fakeStartCancelButton, _sut, _fakeDisplay, _fakeLight, _fakeCoockController);

          
        }


        [Test]
        public void OnDoorOpen_OpenDoor_LightIsTurnedOn()
        {
            //act
            _sut.Open();
            
            //Assert
            _fakeLight.Received(1).TurnOn();

        }

        [Test]
        public void OnDoorClosed_CloseDoor_LightIsTurnedOff()
        {
            //act
            _sut.Open();
            _sut.Close();

            //Assert
            _fakeLight.Received(1).TurnOff();
        }

        [Test]
        public void OpenDoor_ExtensionDoorOpenedBeforeTimeWasSet_Stop()
        {
            //Act
            fakePowerButton.Pressed += Raise.Event();
            _sut.Open();

            //Assert
            _fakeLight.Received(1).TurnOn();
        }


        [Test]
        public void OpenDoor_ExtensionDoorOpenedBeforeStartWasPressed_Stop()
        {
            //Act
            fakePowerButton.Pressed += Raise.Event();
            fakeTimeButton.Pressed += Raise.Event();

            _sut.Open();

            //Assert
            _fakeLight.Received(1).TurnOn();
        }

        [Test]
        public void OpensDoor_ExtensionDoorOpenedWhileCooking_Stop()
        {
            //Act
            fakePowerButton.Pressed += Raise.Event();

            fakeTimeButton.Pressed += Raise.Event();

            fakeStartCancelButton.Pressed += Raise.Event();

            _sut.Open();

            //Assert
            _fakeCoockController.Received(1).Stop();
        }
    }
    
}