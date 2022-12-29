using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        IWCFService factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address):base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void DodajKoncert()
        {
            throw new NotImplementedException();
        }

        public void IzmeniKoncert()
        {
            throw new NotImplementedException(); ;
        }

        public void NapraviRezervaciju()
        {
            throw new NotImplementedException();
        }

        

        public void PlatiRezervaciju()
        {
            throw new NotImplementedException();
        }
    }
}
