using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class TD_ITS4_Light_UI
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IDisplay fakeDisplay;
        private ILight sut;
        private ICookController _cookController;
        private IUserInterface _UI;
        private IPowerTube fakepowerTube;
        private ITimer fakeTimer;
        private IOutput fakeOutput;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();

            fakeDisplay = Substitute.For<IDisplay>();
            fakeTimer = Substitute.For<ITimer>();
            fakepowerTube = Substitute.For<IPowerTube>();
            fakeOutput = Substitute.For<IOutput>();
            sut = new Light(fakeOutput);

            _cookController = new CookController(fakeTimer, fakeDisplay, fakepowerTube);
            _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, fakeDisplay, sut, _cookController);

            _cookController.UI = _UI;
        }

        [Test]
        public void Output_MicrowaveReady_LogLineIsCalled()
        {
            //Act
            _door.Open();

            //Assert
            //Vi kender outputline med vores "White Box" briller
            //fakeOutput.Received(1).OutputLine("Light is turned on");

            //Vi tager turned on med her, så den er mere robut, men stadig forskellig fra TurnOff()
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("turned on")));
        }
        [Test]
        public void Output_DoorOpenGetsClosed_LogLineIsCalled()
        {
            //Act
            _door.Open();
            _door.Close();

            //Assert
            //Vi kender outputline med vores "White Box" briller
            //fakeOutput.Received(1).OutputLine("Light is turned off");

            //Vi tager turned off med her, så den er mere robut, men stadig forskellig fra TurnOn()
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("turned off")));
        }
        [Test]
        public void Outout_CookingIsStarted_LogLineIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            //fakeOutput.Received(1).OutputLine("Light is turned on");

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("turned on")));
        }
        [Test]
        public void Outout_CancelPressedWhileCooking_LogLineIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _startCancelButton.Press();

            //Assert
            //fakeOutput.Received(1).OutputLine("Light is turned off");

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("turned off")));
        }

        [Test]
        public void Output_TimeHaveTicked_LightIsTurnedOff()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            fakeTimer.Expired += Raise.Event();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("turned off")));
        }
    }
}