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
      private IDoor fakeDoor;
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

         fakeDoor = Substitute.For<IDoor>();
         fakeDisplay = Substitute.For<IDisplay>();
         fakeCooker = Substitute.For<ICookController>();

         UI = new UserInterface(powerButton, timeButton, startCancelButton, fakeDoor, fakeDisplay, fakeLight,
            fakeCooker);
      }

      [Test]
      public void OnDoorOpened_Opendoor_LightGoesOn()
      {

      }
   }
}