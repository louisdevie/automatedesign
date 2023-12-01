using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Documents
{
    /// <summary>
    /// Représente un nouveau document qui n'a pas encore été créé.
    /// </summary>
    public class NewDocumentViewModel : DocumentBaseViewModel
    {
        public override string Name => "Nouveau";

        public override string TimeSinceLastModification => "";
    }
}
