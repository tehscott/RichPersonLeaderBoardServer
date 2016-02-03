using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Processor;

namespace ManualProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            WealthResetter resetter = new WealthResetter(new Dao());
            
            resetter.ResetWealth();
        }
    }
}
