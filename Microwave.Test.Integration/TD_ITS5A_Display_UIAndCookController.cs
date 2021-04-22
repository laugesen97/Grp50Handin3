using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
   [TestFixture]
   public class TD_ITS5A_Display_UIAndCookController
   {
      private IButton _powerButton;
      private IButton _timeButton;
      private IButton _startCancelButton;
      private IDoor _door;
      private IDisplay sut;
      private ILight light;
      private ICookController _cookController;
      private IUserInterface _UI;
      private IPowerTube fakepowerTube;
      private ITimer fakeTimer;
      private IOutput fakeOutput;

      [SetUp]
      public void Setup()
      {
         fakeTimer = Substitute.For<ITimer>();
         fakepowerTube = Substitute.For<IPowerTube>();
         fakeOutput = Substitute.For<IOutput>();


         _powerButton = new Button();
         _timeButton = new Button();
         _startCancelButton = new Button();
         _door = new Door();
         sut = new Display(fakeOutput);
         light = new Light(fakeOutput);

         _cookController = new CookController(fakeTimer, sut, fakepowerTube);
         _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, sut, light, _cookController);

         _cookController.UI = _UI;
      }

      [Test]
      public void Output_ShowPower_LogLineIsCalled()
      {
         //Act
         _powerButton.Press();

         //Assert
         //Vi kender outputline med vores "White Box" briller
         //fakeOutput.Received(1).OutputLine("Display shows: 50 W");
         //Vi tager turned on med her, så den er mere robut, men stadig forskellig fra TurnOff()
         fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows:") && s.Contains("W")));
      }

      [Test]
      public void Output_ShowTime_LogLineIsCalled()
      {
         //Act
         _powerButton.Press();
         
         _timeButton.Press();

         //Assert
         //Vi kender outputline med vores "White Box" briller
         fakeOutput.Received(1).OutputLine("Display shows: 01:00 min");
         //Vi tager turned off med her, så den er mere robut, men stadig forskellig fra TurnOn()
         fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows:") && s.Contains("min") ));
      }

      [Test]
      public void Outout_CookingAndDoorOpened_LogLineIsCalled()
      {
         //Navngivningen af metoden skal vi have kigget på. :)

         //Arrange

         //Act
         _powerButton.Press();

         _timeButton.Press();

         _startCancelButton.Press();

         _door.Open();

         //Assert
         fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));
      }

      [Test]
      public void Outout_CancelPressedWhileCooking_LogLineIsCalled()
      {
         //Navngivningen af metoden skal vi have kigget på. :)

         //Arrange

         //Act
         _powerButton.Press();

         _timeButton.Press();

         _startCancelButton.Press();

         _startCancelButton.Press();

         //Assert
         fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));
      }
   }
}
