using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomateDesign.Core.Random;

namespace TestsUnitaires.Core.Random
{
    public class BasicRandomProviderTest
    {
        [Fact]
        public void TestFourDigitCode()
        {
            BasicRandomProvider basicProvider = new BasicRandomProvider(); 
            uint result = basicProvider.FourDigitCode();
            Assert.True(result >= 0 && result < 10000);
        }

        [Fact]
        public void TestPick()
        {
            BasicRandomProvider basicProvider = new BasicRandomProvider();
            char result = basicProvider.Pick("allowedChars");
            Assert.True("allowedChars".Contains(result.ToString()));
        }
    }
}
