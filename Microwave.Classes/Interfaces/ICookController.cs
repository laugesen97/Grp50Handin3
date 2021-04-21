using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Classes.Interfaces
{
    public interface ICookController
    {
        //HEHE FRANK VI HAR FUNDET DIN FEJL!!! DET ER ØVERSTE LINJE HER, HUSK AT UDKOMMENTERE :)))
        IUserInterface UI { set;}




        void StartCooking(int power, int time);
        void Stop();
    }
}
