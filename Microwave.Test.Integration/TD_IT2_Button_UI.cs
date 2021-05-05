using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
   [TestFixture]
   public class TD_IT2_Button_UI
   {
      private IButton powerButton;
      private IButton timeButton;
      private IButton startCancelButton;
      private IDoor Door;
      private IDisplay fakeDisplay;
      private ILight fakeLight;
      private ICookController fakeCooker;
      private IUserInterface UI;

      [SetUp]
      public void Setup()
      {
         powerButton = new Button();
         timeButton = new Button();
         startCancelButton = new Button();
         Door = new Door();

         fakeDisplay = Substitute.For<IDisplay>();
         fakeCooker = Substitute.For<ICookController>();
         fakeLight = Substitute.For<ILight>();

         UI = new UserInterface(powerButton, timeButton, startCancelButton, Door, fakeDisplay, fakeLight,
            fakeCooker);
      }

      [Test]
      public void Press_PowerButtonPressedByUser_ShowPowerIsCalled()
      {
         //Act
         powerButton.Press();

         //Assert
         fakeDisplay.Received(1).ShowPower(50);
      }


      [Test]
      public void Press_TimeButtonPressedByUser_ShowTimeIsCalled()
      {
         //Act
         powerButton.Press();

         timeButton.Press();

         //Assert
         fakeDisplay.Received(1).ShowTime(1,0);
      }

      [Test]
      public void Press_CancelButtonPressedByUserBeforeTimeButton_Cancel()
      {
          //Act
          powerButton.Press();

          startCancelButton.Press();

          //Assert
          fakeDisplay.Received(1).Clear();
        }

      [Test]
      public void Press_StartButtonPressedByUser_StartIsCalled()
      {
         //Act
         powerButton.Press();

         timeButton.Press();

         startCancelButton.Press();

         //Assert
         fakeLight.Received(1).TurnOn();
      }

      [Test]
      public void Press_CancelButtonPressedByUser_CancelIsCalled()
      {
          //Act
          powerButton.Press();

          timeButton.Press();

          startCancelButton.Press();

          startCancelButton.Press();

          //Assert
          fakeLight.Received(1).TurnOff();
        }

      [Test]
      public void Press_StartButtonPressedByUser_StartCookingIsCalled()
      {
          //Act
          powerButton.Press();

          timeButton.Press();

          startCancelButton.Press();

          //Assert
          //50 og 60 kender vi, da vi har kigget med "White box" øjne
          fakeCooker.Received(1).StartCooking(50,60);
        }

      [Test]
      public void Press_CancelButtonPressedByUser_StopIsCalled()
      {
          //Act
          powerButton.Press();

          timeButton.Press();

          startCancelButton.Press();

          startCancelButton.Press();

          //Assert
          fakeCooker.Received(1).Stop();
      }
    }
}