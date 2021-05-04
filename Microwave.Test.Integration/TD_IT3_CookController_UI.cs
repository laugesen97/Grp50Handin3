using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NuGet.Frameworks;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class TD_IT3_CookController_UI
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IDisplay fakeDisplay;
        private ILight fakeLight;
        private CookController sut;
        private IUserInterface _UI;
        private IPowerTube fakepowerTube;
        private ITimer fakeTimer;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();

            fakeDisplay = Substitute.For<IDisplay>();
            fakeLight = Substitute.For<ILight>();
            fakeTimer = Substitute.For<ITimer>();
            fakepowerTube = Substitute.For<IPowerTube>();

            sut = new CookController(fakeTimer, fakeDisplay, fakepowerTube);
            _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, fakeDisplay, fakeLight, sut);

            sut.UI = _UI;
        }

        [Test]
        public void Timer_CookingIsStartedWith60Sec_StartIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //60 kendes igen fra tidligere White Box undersøgelse.
            fakeTimer.Received(1).Start(60);
        }

        [Test]
        public void Timer_CookingIsStartedWith120Sec_StartIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //120 kendes fra tidligere White Box undersøgelse..
            fakeTimer.Received(1).Start(120);
        }

        [Test]
        public void Timer_CookingIsStartedWith180Sec_StartIsCalled()
        {

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _timeButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //180 kendes fra tidligere White Box undersøgelse..
            fakeTimer.Received(1).Start(180);
        }

        [Test]
        public void Timer_CookingIsStarted_StartIsNotCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            fakeTimer.Received(0).Start(30);
        }

        [Test]
        public void PowerTube_CookingIsStartedWith50Watt_TurnOnIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //50 kendes fra tidligere White Box undersøgelse.
            fakepowerTube.Received(1).TurnOn(50);
        }

        [Test]
        public void PowerTube_CookingIsStartedWith100Watt_TurnOnIsCalled()
        {
            //Act
            _powerButton.Press();

            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //100 kendes fra tidligere White Box undersøgelse.
            fakepowerTube.Received(1).TurnOn(100);
        }

        [Test]
        public void PowerTube_CookingIsStartedWith150Watt_TurnOnIsCalled()
        {
            //Act
            _powerButton.Press();

            _powerButton.Press();

            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //150 kendes fra tidligere White Box undersøgelse.
            fakepowerTube.Received(1).TurnOn(150);
        }

        [Test]
        public void Display_Cooked1Minuted_ShowTimeCalled60Times()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //1 minut kendes fra tidligere White Box undersøgelse.
            fakeDisplay.Received(1).ShowTime(1, 0);
        }

        [Test]
        public void Timer_DoorOpens_StopIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _door.Open();

            //Assert
            fakeTimer.Received(1).Stop();
        }

        [Test]
        public void Timer_CancelPressed_StopIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _startCancelButton.Press();

            //Assert
            fakeTimer.Received(1).Stop();
        }

        [Test]
        public void PowerTube_DoorOpens_TurnOffIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _door.Open();

            //Assert
            fakepowerTube.Received(1).TurnOff();
        }

        [Test]
        public void PowerTube_CancelPressed_TurnOffIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _startCancelButton.Press();

            //Assert
            fakepowerTube.Received(1).TurnOff();
        }

        [Test]
        public void PowerTube_TimeHaveTicked_TurnOffIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            fakeTimer.Expired += Raise.Event();

            //Assert
            fakepowerTube.Received(1).TurnOff();
        }

        [Test]
        public void Light_TimeHaveTicked_TurnOffIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            fakeTimer.Expired += Raise.Event();

            //Assert
            fakeLight.Received(1).TurnOff();
        }

        [Test]
        public void Display_TimeHaveTicked_ClearIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            fakeTimer.Expired += Raise.Event();

            //Assert
            fakeDisplay.Received(1).Clear();
        }
    }
}