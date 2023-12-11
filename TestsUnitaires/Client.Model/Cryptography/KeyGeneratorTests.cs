using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Cryptography
{
    public class KeyGeneratorTests
    {
        [Fact]
        public void Pbkdf2()
        {
            byte[] k1 = Pbkdf2KeyGenerator.GetKey(16, "motdepasse", "automatedesign");
            byte[] k2 = Pbkdf2KeyGenerator.GetKey(16, "motdepasse", "automatedesign");
            byte[] k3 = Pbkdf2KeyGenerator.GetKey(16, "abc", "def");
            byte[] k4 = Pbkdf2KeyGenerator.GetKey(16, "motdepasse", "eehfbZEBFJCQNqjcnkqJQDCNKqjcnbzehjBJFEB");

            Assert.Equal(16, k1.Length);
            Assert.Equal(16, k2.Length);
            Assert.Equal(16, k3.Length);
            Assert.Equal(16, k4.Length);

            Assert.Equal(k1, k2);
            Assert.NotEqual(k1, k3);
            Assert.NotEqual(k1, k4);
        }
    }
}
