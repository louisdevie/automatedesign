using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    public class DocumentSerializer
    {
        public byte[] SerializeToUtf8Bytes(Document document)
        {
            // Sérialiser la classe en bytes au format UTF-8
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(document);
            return jsonUtf8Bytes;
        }
    }
}
