using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;
using ShopOnlineCore.Utils;

namespace ShopOnlineCore.UseCase
{
    public class ProductUC
    {
        private IUnitOfWork _unitOfWork;
        private EventHandlerCore _handlerEvent = new();

        public ProductUC(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Product> List(object? search = null, int page = 1, int itemPerPage = 25)
        {
            var result = _unitOfWork.Product.Find(search, page, itemPerPage);
            _handlerEvent.RealeaseEvent(Constants.EventCore.ReadProduct, new object[] { result });
            return result;
        }
        public Product GetById(string id)
        {
            var result = _unitOfWork.Product.FindOne(id);
            if (result == null)
                throw new NotFoundException("Product not found.");
            _handlerEvent.RealeaseEvent(Constants.EventCore.ReadProduct, new object[] { result });
            return result;
        }
        public Product Add(Product product)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                product.Id = BaseEntity.CreateUUID();
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                _handlerEvent.RealeaseEvent(Constants.EventCore.AddProduct, new object[] { product });
                _unitOfWork.CommitTransaction();
                return product;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(Product product)
        {
            if (string.IsNullOrEmpty(product.Id))
                throw new ArgumentNullException("Id", "Id is required.");
            var model = _unitOfWork.Product.FindOne(product.Id) ?? throw new NotFoundException("Product not found.");
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            _handlerEvent.RealeaseEvent(Constants.EventCore.UpdateProduct, new object[] { product });
        }
        public void Remove(string id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var model = _unitOfWork.Product.FindOne(id);
                if (model == null)
                    throw new NotFoundException("Product not found.");

                _unitOfWork.Product.Remove(model);
                _unitOfWork.Save();
                _handlerEvent.RealeaseEvent(Constants.EventCore.DeleteProduct, new object[] { model });
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
