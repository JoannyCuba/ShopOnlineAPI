using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;
using ShopOnlineCore.Utils;

namespace ShopOnlineCore.UseCase
{
    public class ClientUC
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EventHandlerCore _handlerEvent = new();

        /// <summary>
        /// Initializes a new instance of the ClientUC class.
        /// </summary>
        public ClientUC(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves a paginated list of clients filtered by the specified search criteria and notifies subscribers via an event.
        /// </summary>
        /// <param name="search">Optional search criteria for filtering clients; pass null to retrieve all clients.</param>
        /// <param name="page">The page number to retrieve, starting at 1.</param>
        /// <param name="itemPerPage">The number of clients to return per page.</param>
        /// <returns>A list of clients matching the search and pagination parameters.</returns>
        /// <remarks>
        /// An event is triggered with the retrieved client list to notify subscribers of the read operation.
        /// </remarks>
        public List<Client> List(object? search = null, int page = 1, int itemPerPage = 25)
        {
            var result = _unitOfWork.Client.Find(search, page, itemPerPage);
            _handlerEvent.RealeaseEvent(Constants.EventCore.ReadClient, new object[] { result });
            return result;
        }
        public Client GetById(string id)
        {
            var result = _unitOfWork.Client.FindOne(id) ?? throw new NotFoundException("Client not found.");
            _handlerEvent.RealeaseEvent(Constants.EventCore.ReadClient, new object[] { result });
            return result;
        }
        public Client Add(Client client)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                client.Validate();

                client.Id = BaseEntity.CreateUUID();
                _unitOfWork.Client.Add(client);
                _unitOfWork.Save();
                _handlerEvent.RealeaseEvent(Constants.EventCore.AddClient, new object[] { client });
                _unitOfWork.CommitTransaction();
                return client;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(Client client)
        {
            if (string.IsNullOrEmpty(client.Id))
                throw new ArgumentNullException("Id", "Id is required.");
            client.Validate();
            _ = _unitOfWork.Client.FindOne(client.Id) ?? throw new NotFoundException("Client not found.");
            _unitOfWork.Client.Update(client);
            _unitOfWork.Save();
            _handlerEvent.RealeaseEvent(Constants.EventCore.UpdateClient, new object[] { client });
        }
        public void Remove(string id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var model = _unitOfWork.Client.FindOne(id) ?? throw new NotFoundException("Client not found.");
                _unitOfWork.Client.Remove(model);
                _unitOfWork.Save();
                _handlerEvent.RealeaseEvent(Constants.EventCore.DeleteClient, new object[] { model });
                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
