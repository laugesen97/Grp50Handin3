using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
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

        [SetUp]
        public void SetUp()
        {
            _fakeCoockController = Substitute.For<ICookController>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeLight = Substitute.For<ILight>();

            _userInterface = new UserInterface();

            _sut = new Door();

        }


        






    }
    
}