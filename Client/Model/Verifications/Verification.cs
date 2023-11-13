using AutomateDesign.Client.Model.Network;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Verifications
{
    public abstract class Verification
    {
        public abstract string Title { get; }

        public abstract string SuccessMessage { get; }

        public abstract Task SendVerificationRequest(UsersClient client, uint secretCode);
    }
}
