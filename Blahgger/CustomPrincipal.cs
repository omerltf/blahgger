using Blahgger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Blahgger
{

    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsCurrentUser(int id)
        {
            if (id == 0) return false;
            //if currentLoggedinUserId == id return true

            return false;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email, "Custom");
        }
        public int Id { get; set; }
    }
}