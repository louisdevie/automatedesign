using AutomateDesign.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUnitaires.Core.Random
{
    public class CryptoRandomProviderTest
    {
        [Fact]
        public void TestFourDigitCode()
        {
            CryptoRandomProvider basicProvider = new CryptoRandomProvider();
            uint result = basicProvider.FourDigitCode();
            Assert.True(result >= 0 && result < 10000);
        }

        [Fact]
        public void TestPick()
        {
            CryptoRandomProvider basicProvider = new CryptoRandomProvider();
            char result = basicProvider.Pick("allowedChars");
            Assert.True("allowedChars".Contains(result.ToString()));
        }
    }
}
