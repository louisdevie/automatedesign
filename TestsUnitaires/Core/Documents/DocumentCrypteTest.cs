using AutomateDesign.Core.Documents;
using Xunit;

namespace TestsUnitaires.Core.Documents
{
    public class DocumentCrypteTest
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            DocumentCrypte documentCrypte = new DocumentCrypte();

            // Assert
            Assert.Equal(0, documentCrypte.IdDoc);
            Assert.Null(documentCrypte.Document);
            Assert.Equal(0, documentCrypte.IdUser);
        }

        [Fact]
        public void ParameterizedConstructor_SetsProperties()
        {
            // Arrange
            int idDocument = 1;
            byte[] document = new byte[] { 1, 2, 3 };
            int idUser = 2;

            // Act
            DocumentCrypte documentCrypte = new DocumentCrypte(idDocument, document, idUser);

            // Assert
            Assert.Equal(idDocument, documentCrypte.IdDoc);
            Assert.Equal(document, documentCrypte.Document);
            Assert.Equal(idUser, documentCrypte.IdUser);
        }

        [Fact]
        public void WithId_ReturnsNewDocumentCrypteWithUpdatedId()
        {
            // Arrange
            int originalId = 1;
            byte[] document = new byte[] { 1, 2, 3 };
            int idUser = 2;
            DocumentCrypte originalDocumentCrypte = new DocumentCrypte(originalId, document, idUser);

            // Act
            int newId = 3;
            DocumentCrypte newDocumentCrypte = originalDocumentCrypte.WithId(newId);

            // Assert
            Assert.Equal(newId, newDocumentCrypte.IdDoc);
            Assert.Equal(document, newDocumentCrypte.Document);
            Assert.Equal(idUser, newDocumentCrypte.IdUser);
        }
    }
}
