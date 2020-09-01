using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WizardScoreboard
{
    public interface IAuth
    {
        Task<string> LoginWithEmailPassword(string username, string password);
    }
}
