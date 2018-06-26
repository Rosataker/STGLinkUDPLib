using System;
using System.Collections.Generic;
using System.Text;

namespace ISTUDP
{
    public interface ISTUDPInterface
    {
        bool Connected { get; }
        void Open();
        void Destructor();


    }
}
