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
            _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, fakeDisplay, fakeLight,sut);

            sut.UI = _UI;
        }

        [Test]
        public void Timer_CookingIsStarted_StartIsCalled()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //60 kendes igen fra tidligere White Box undersøgelse..
            fakeTimer.Received(1).Start(60);
        }
        [Test]
        public void PowerTube_CookingIsStarted_TurnOnIsCalled()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //50 kendes igen fra tidligere White Box undersøgelse..
            fakepowerTube.Received(1).TurnOn(50);
        }
        [Test]
        public void Display_Cooked1Minuted_ShowTimeCalled60Times()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //60 kendes igen fra tidligere White Box undersøgelse..
            fakeDisplay.Received(1).ShowTime(1,0);
        }
        [Test]
        public void Timer_DoorOpens_StopIsCalled()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange
            
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _door.Open();

            fakeTimer.Received(1).Stop();
        }
        [Test]
        public void Timer_CancelPressed_StopIsCalled()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _startCancelButton.Press();

            fakeTimer.Received(1).Stop();
        }
        [Test]
        public void PowerTube_DoorOpens_TurnOffIsCalled()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _door.Open();

            fakepowerTube.Received(1).TurnOff();
        }
        [Test]
        public void PowerTube_CancelPressed_TurnOffIsCalled()
        {
            //Navngivningen af metoden skal vi have kigget på. :)

            //Arrange

            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _startCancelButton.Press();

            fakepowerTube.Received(1).TurnOff();
        }
    }
}