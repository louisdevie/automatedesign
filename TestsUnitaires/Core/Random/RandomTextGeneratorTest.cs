using AutomateDesign.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUnitaires.Core.Random
{
    public class RandomTextGeneratorTest
    {
        private IRandomProvider provider  = new BasicRandomProvider();
                private const string ALPHANUM = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        [Fact]
        public void TestAlphaNumericString()
        {
            RandomTextGenerator randomText = new RandomTextGenerator(provider);
            int size = 8;
            string result = randomText.AlphaNumericString(size);
            Assert.Equal(size, result.Length);
            Assert.True(IsAlphaNumeric(result));
        }

        [Fact]
        public void TestSecretKey()
        {
            RandomTextGenerator randText = new RandomTextGenerator(provider);
            string result = randText.SecretKey(); 
            Assert.Equal(30, result.Length);
            Assert.True(IsCorrectSecretKeyFormat(result));
        }

        private bool IsAlphaNumeric(string str)
        {
            bool test = true;
            foreach (char c in str)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    test = false;
                }
            }
            return test;
        }

        private bool IsCorrectSecretKeyFormat(string str)
        {
            bool test = true;
            foreach (char c in str)
            {
                if (!(ALPHANUM.Contains(c) || c=='-'))
                {
                    test = false;
                }
            }

            return test;
        }
    }
}
