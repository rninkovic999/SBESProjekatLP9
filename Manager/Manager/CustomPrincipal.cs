using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class CustomPrincipal:IPrincipal
    {
        WindowsIdentity identity = null;
        public CustomPrincipal(WindowsIdentity windowsIdentity)
        {
            identity = windowsIdentity;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            Type x509IdentityType = identity.GetType();

            FieldInfo certificateField = x509IdentityType.GetField("certificate", BindingFlags.Instance | BindingFlags.NonPublic);

            X509Certificate2 certificate = (X509Certificate2)certificateField.GetValue(identity);

            string name = certificate.SubjectName.Name;
            string[] clientName = name.Split(';');
            string[] parts = clientName[0].Split(';');
            string[] roleName = parts[1].Split('=');

            if(role.Equals(roleName[1]))
            {
                return true;
            }
            return false;
        }
    }
}
