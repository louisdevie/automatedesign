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
            IKeyGenerator keyGenerator = new Pbkdf2KeyGenerator();

            byte[] k1 = keyGenerator.GetKey(16, "motdepasse", "automatedesign");
            byte[] k2 = keyGenerator.GetKey(16, "motdepasse", "automatedesign");
            byte[] k3 = keyGenerator.GetKey(16, "abc", "def");
            byte[] k4 = keyGenerator.GetKey(16, "motdepasse", "eehfbZEBFJCQNqjcnkqJQDCNKqjcnbzehjBJFEB");

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
