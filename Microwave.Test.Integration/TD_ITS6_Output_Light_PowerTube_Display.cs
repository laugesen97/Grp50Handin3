using System;
using System.IO;
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
   public class TD_ITS6_Output_Light_PowerTube_Display
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
      private ITimer faketimer;
      private IOutput sut;

      [SetUp]
      public void Setup()
      {
         faketimer = Substitute.For<ITimer>();

         sut = new Output();
         _powerButton = new Button();
         _timeButton = new Button();
         _startCancelButton = new Button();
         _door = new Door();

         display = new Display(sut);
         light = new Light(sut);
         powerTube = new PowerTube(sut);

         _cookController = new CookController(faketimer, display, powerTube);
         _UI = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, display, light, _cookController);

         _cookController.UI = _UI;
      }

      [Test]
      public void OutputLine_DoorIsOpened_LogLineIsCalled()
      {
         //Arrange
         var output = new StringWriter();
         Console.SetOut(output);

         //Act
         _door.Open();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Light is turned on\r\n"));
      }

      [Test]
      public void OutputLine_DoorIsOpenedAndClosed_LogLineIsCalled()
      {
         //Arrange
         var output = new StringWriter();


         //Act
         _door.Open();
         Console.SetOut(output);
         _door.Close();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Light is turned off\r\n"));
      }

      [Test]
      public void OutputLine_PowerIsPressed_LogLineIsCalled()
      {
         //Arrange
         var output = new StringWriter();
         Console.SetOut(output);

         //Act
         _powerButton.Press();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Display shows: 50 W\r\n"));
      }

      [Test]
      public void OutputLine_TimeButtonIsPressed_LogLineIsCalled()
      {
         //Arrange

         var output = new StringWriter();


         //Act
         _powerButton.Press();
         Console.SetOut(output);
         _timeButton.Press();


         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Display shows: 01:00 min\r\n"));
      }

      [Test]
      public void OutputLine_StartCancelButtonIsPressed_LogLineIsCalled()
      {
         //Arrange

         var output = new StringWriter();


         //Act
         _powerButton.Press();
         _timeButton.Press();
         Console.SetOut(output);
         _startCancelButton.Press();


         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Light is turned on\r\nPowerTube works with 50 W\r\n"));
      }


      [Test]
      public void OutputLine_CookingIsDone_LogLineIsCalled()
      {
         //Arrange
         var output = new StringWriter();


         //Act
         _powerButton.Press();
         _timeButton.Press();
         _startCancelButton.Press();

         Console.SetOut(output);
         faketimer.Expired += Raise.Event();

         string outputstring = output.ToString();
         //Sammenlign med output..
         //Assert
         Assert.That(outputstring.Contains("PowerTube turned off") && outputstring.Contains("Display cleared") &&
                     outputstring.Contains("Light is turned off"));

      }
      [Test]
      public void OutputLine_DoorOpenedWhenSettingPower_LogLineIsCalled()
      {
         //Arrange
         var output = new StringWriter();

         //Act
         _powerButton.Press();
         Console.SetOut(output);
         _door.Open();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Light is turned on\r\nDisplay cleared\r\n"));
      }
      [Test]
      public void OutputLine_DoorOpenedWhenSettingTime_LogLineIsCalled()
      {
         //Arrange

         var output = new StringWriter();


         //Act
         _powerButton.Press();
         _timeButton.Press();
         Console.SetOut(output);
         _door.Open();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("Light is turned on\r\nDisplay cleared\r\n"));
      }

      [Test]
      public void OutputLine_DoorIsOpenedWhileCooking_LogLineIsCalled()
      {
         //Arrange
         var output = new StringWriter();


         //Act
         _powerButton.Press();
         _timeButton.Press();
         _startCancelButton.Press();
         Console.SetOut(output);
         _door.Open();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("PowerTube turned off\r\nDisplay cleared\r\n"));
      }

      [Test]
      public void OutputLine_CancelIsPressedWhileCooking_LogLineIsCalled()
      {
         //Arrange


         var output = new StringWriter();


         //Act
         _powerButton.Press();
         _timeButton.Press();
         _startCancelButton.Press();

         faketimer.TimerTick += Raise.Event();
         faketimer.TimerTick += Raise.Event();
         faketimer.TimerTick += Raise.Event();
         faketimer.TimerTick += Raise.Event();
         faketimer.TimerTick += Raise.Event();

         Console.SetOut(output);
         _startCancelButton.Press();

         //Assert
         Assert.That(output.ToString(), Is.EqualTo("PowerTube turned off\r\nLight is turned off\r\nDisplay cleared\r\n"));
      }
   }
}