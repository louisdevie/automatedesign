using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUnitaires.Core.Documents
{
    public class DocumentSerializerTests
    {
        [Fact]
        public void SerializeToUtf8BytesTest()
        {
         
            var document = new Document();
            var documentSerializer = new DocumentSerializer();

        
            byte[] result = documentSerializer.SerializeToUtf8Bytes(document);
           
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
