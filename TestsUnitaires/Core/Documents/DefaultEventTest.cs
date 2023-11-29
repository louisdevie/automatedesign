using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUnitaires.Core.Documents
{
    public class DefaultEventTest
    {
        [Fact]
        public void TestOrder()
        {
            DefaultEvent defaultEvent = new DefaultEvent();
            Assert.Equal(defaultEvent.Order, 1_000_000_000);
        }

        [Fact]
        public void TestDisplayName()
        {
            DefaultEvent defaultEvent = new DefaultEvent();
            Assert.Equal(defaultEvent.DisplayName, "*");
        }
    }
}
