using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;
using ShopOnlineCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.UseCase
{
    public class ClientUC
    {
        private IUnitOfWork _unitOfWork;
        private EventHandlerCore _handlerEvent = new();

        public ClientUC(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Client> List(object search = null, int page = 1, int itemPerPage = 25)
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
