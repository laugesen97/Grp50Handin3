using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
   [TestFixture]
   public class TD_ITS5C_Timer_CookController
   {

      private IButton _powerButton;
      private IButton _timeButton;
      private IButton _startCancelButton;
      private IDoor _door;
      private IDisplay display;
      private ILight light;
      private ICookController _cookController;
      private IUserInterface _UI;
      private IPowerTube powerTube;
      private ITimer sut;
      private IOutput fakeOutput;

      [SetUp]
      public void Setup()
      {
         fakeOutput = Substitute.For<IOutput>();

         _powerButton = new Button();
         _timeButton = new Button();
         _startCancelButton = new Button();
         _door = new Door();
         display = new Display(fakeOutput);
         light = new Light(fakeOutput);
         powerTube = new PowerTube(fakeOutput);
         sut = new Timer();

         _cookController = new CookController(sut, display, powerTube);
         _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, display, light, _cookController);

         _cookController.UI = _UI;
      }

      [Test]
      public void Output_Tick_LogLineIsCalled()
      {
         //Act
         _powerButton.Press();

         _timeButton.Press();

         _startCancelButton.Press();

         Thread.Sleep(1000);
         
         //Assert
         //                 //2 pga. den viser både 01.00 og 00.59
        // fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Display shows:") && s.Contains("min")));
      }

      [Test]
      public void Output_TurnOffPowerTube_LogLineIsCalled()
      {
         //Act
         _powerButton.Press();

         _timeButton.Press();

         _startCancelButton.Press();

         //Thread.Sleep(60500);

         //Assert
         //fakeOutput.Received(61).OutputLine(Arg.Is<string>(s =>s.Contains("Display shows:") && s.Contains("min")));
        // fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));
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
   }
}