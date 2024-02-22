using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;
using ShopOnlineCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.UseCase
{
    public class SaleUC
    {
        private IUnitOfWork _unitOfWork;
        private EventHandlerCore _handlerEvent = new();
        public SaleUC(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Sale> List(object search = null, int page = 1, int itemPerPage = 25)
        {
            var result = _unitOfWork.Sale.Find(search, page, itemPerPage);
            _handlerEvent.RealeaseEvent(Constants.EventCore.ReadSale, new object[] { result });
            return result;
        }
        public Sale Add(Sale sale)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                //sale.Id = BaseEntity.CreateUUID();
                _unitOfWork.Sale.Add(sale);
                _unitOfWork.Save();
                _handlerEvent.RealeaseEvent(Constants.EventCore.AddSale, new object[] { sale });
                _unitOfWork.CommitTransaction();
                return sale;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
