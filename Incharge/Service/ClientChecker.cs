using Incharge.Models;
using Incharge.Repository;
using Incharge.Service.IService;
using Incharge.Repository.IRepository;

namespace Incharge.Service
{
    public class ClientChecker : IChecker<Client>
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<Client> _ClientRepository;
 
        public ClientChecker(IFindRepository<Client> FindClientRepository, IRepository<Client> ClientRepository)
        {
            _FindClientRepository = FindClientRepository;
            _ClientRepository = ClientRepository;

        }
        //seriously need to maek async
        public void Check()
        {
            var ListAllClients = _FindClientRepository.ListBy(x => x.Id > 0);
            foreach (var client in ListAllClients)
            {
                if(client.MembershipProductId != 0) //check if they even have a membership
                {
                    if (client.MembershipExpiryDate < DateTime.Now)
                    {
                        client.MembershipStatus = "Overdue";
                        _ClientRepository.Update(client);
                    }
                }
            }
           
        }
    }
}
