using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Utils;
using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;
using ShopOnlineCore.UseCase;
using ShopOnlineCore.Utils;

namespace ShopOnlineAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing sales.
    /// </summary>
    [Route("api/sale")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventTrace _eventTrace;
        private readonly ClientUC _clientUC;
        private readonly ProductUC _productUC;
        private readonly SaleUC _saleUC;

        /// <summary>
        /// SaleController Constructor.
        /// </summary>
        public SaleController(IUnitOfWork unitOfWork, IEventTrace eventTrace)
        {
            _unitOfWork = unitOfWork;
            _eventTrace = eventTrace;
            _clientUC = new ClientUC(unitOfWork);
            _productUC = new ProductUC(unitOfWork);
            _saleUC = new SaleUC(unitOfWork);
        }

        /// <summary>
		/// Returns the Sale list.
		/// </summary>
		/// <param name="search"></param>
		/// <param name="page"></param>
		/// <param name="itemPerPage"></param>
		/// <returns></returns>
        [HttpGet]
        public ApiResult Get(string? search, int page = 1, int itemPerPage = 25)
        {
            var filter = new FilterDto() { search = search };

            var result = _saleUC.List(filter, page, itemPerPage);
            int total = _unitOfWork.Client.Count(filter);
            return ApiResult.Success(new
            {
                items = result.Select(x =>
                    AutoMapperProfile.Map<Sale, SaleModel>(x, true)
                ).ToList(),
                total,
            });
        }
        /// <summary>
        /// Create a Sale to a Client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sellproduct")]
        public ApiResult SellProduct(string clientId, string productId, int quantity)
        {
            // Busco el cliente y el producto
            var client = _clientUC.GetById(clientId);
            var product = _productUC.GetById(productId);

            if (client == null || product == null)
            {
                return ApiResult.NotFound("Client or product Not Found");
            }

            // Verifico si hay suficiente stock para la venta
            if (product.InStock < quantity)
            {
                return ApiResult.BadRequest("Not enougth items in stock to sell");
            }

            // Realizar la venta
            SaleModel sale = new()
            {
                SaleDate = DateTime.Now,
                QuantitySold = quantity,
                ClientModelId = clientId,
                ProductModelId = productId,
                Client = AutoMapperProfile.Map<Client, ClientModel>(client, true),
                Product = AutoMapperProfile.Map<Product, ProductModel>(product, true)
            };

            var data = AutoMapperProfile.Map<SaleModel, Sale>(sale, true);
            _saleUC.Add(data);
            _eventTrace.AddTrace(Constants.EventCore.AddSale, data);
            // Actualizo el stock del producto
            product.InStock -= quantity;
            _unitOfWork.Product.Update(product);
            _eventTrace.AddTrace(Constants.EventCore.UpdateProduct, product);
            _unitOfWork.Save();

            return ApiResult.Success();
        }
    }
}