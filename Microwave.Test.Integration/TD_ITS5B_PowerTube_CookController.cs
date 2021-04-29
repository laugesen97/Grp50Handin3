using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class TD_ITS5B_PowerTube_CookController
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IDisplay display;
        private ILight light;
        private ICookController _cookController;
        private IUserInterface _UI;
        private IPowerTube _sut;
        private ITimer fakeTimer;
        private IOutput fakeOutput;

        [SetUp]
        public void Setup()
        {
            fakeTimer = Substitute.For<ITimer>();
            fakeOutput = Substitute.For<IOutput>();

            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            display = new Display(fakeOutput);
            light = new Light(fakeOutput);
            _sut = new PowerTube(fakeOutput);

            _cookController = new CookController(fakeTimer, display, _sut);
            _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, display, light, _cookController);

            _cookController.UI = _UI;
        }

        [Test]
        public void Output_TurnOnPowerTube_LogLineIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube works with") && s.Contains("W")));
        }

        [Test]
        public void Output_TurnOffPowerTube_LogLineIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));
        }

        [Test]
        public void Output_TurnOffPowerTubeByOpenDoor_LogLineIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            _door.Open();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));
        }

        [Test]
        public void Output_TimeHaveTicked_TurnedOffIsCalled()
        {
            //Act
            _powerButton.Press();

            _timeButton.Press();

            _startCancelButton.Press();

            fakeTimer.Expired += Raise.Event();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));
        }

        [Test]
        public void Output_PowerButtonPressed5Times_CookingWith250W()
        {
            //Act
            for (int i = 0; i < 5; i++)
            {
                _powerButton.Press();
            }

            _timeButton.Press();
            _startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube works with") && s.Contains("W")));
        }
        [Test]
        public void Output_PowerButtonPressed5Times_CookingWith700W()
        {
            //Act
            for (int i = 0; i < 13; i++)
            {
                _powerButton.Press();
            }

            _timeButton.Press();
            _startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube works with") && s.Contains("W")));
        }
    }
}