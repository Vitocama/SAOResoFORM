using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOResoForm.Service.IdentityService
{
  
        public interface IIdentity
        {
        bool Autenticato(string utente, string password);
            
        }
    }


