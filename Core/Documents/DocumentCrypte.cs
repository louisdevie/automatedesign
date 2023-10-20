using AutomateDesign.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    public class DocumentCrypte
    {
        #region Attributs
        private int idDoc;
        private byte[] document;
        private int idUser;
        #endregion

        #region Properties
        public int IdDoc { get => this.idDoc; set => this.idDoc = value; }
        public byte[] Document { get => this.document; set => this.document = value; }
        public int IdUser { get => this.idUser; set => this.idUser = value; }
        #endregion

        public DocumentCrypte() { }

        public DocumentCrypte(int idDocument, byte[] document, int idUser) 
        { 
            this.idDoc = idDocument;
            this.document = document;
            this.idUser = idUser;
        }

        public DocumentCrypte WithId(int id) => new(id, this.document, this.idUser);

    }
}
