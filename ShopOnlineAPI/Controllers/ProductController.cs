using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Utils;
using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;
using ShopOnlineCore.UseCase;
using ShopOnlineCore.Utils;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventTrace _eventTrace;
        private readonly ProductUC _productUC;

        public ProductController(IUnitOfWork unitOfWork, IEventTrace eventTrace)
        {
            _unitOfWork = unitOfWork;
            _eventTrace = eventTrace;
            _productUC = new ProductUC(unitOfWork);
        }
        /// <summary>
        /// Returns the Products list.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="itemPerPage"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult Get(string? search, int page = 1, int itemPerPage = 25)
        {
            var filter = new FilterDto() { search = search };

            var result = _productUC.List(filter, page, itemPerPage);
            int total = _unitOfWork.Product.Count(filter);
            return ApiResult.Success(new
            {
                items = result.Select(x =>
                    AutoMapperProfile.Map<Product, ProductDto>(x, true)
                ).ToList(),
                total,
            });
        }
        /// <summary>
		/// Returns the Product by the Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        [HttpGet("{id}")]
        public ApiResult Get(string id)
        {
            var result = _productUC.GetById(id);
            return ApiResult.Success(AutoMapperProfile.Map<Product, ProductDto>(result, true));
        }
        /// <summary>
        /// Create a Product.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ApiResult Add(ProductDto data)
        {
            var entity = AutoMapperProfile.Map<ProductDto, Product>(data, true);
            _productUC.Add(entity);
            _eventTrace.AddTrace(Constants.EventCore.AddProduct, entity);
            return ApiResult.Success();
        }

        /// <summary>
        /// Update a Product.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public ApiResult Update(ProductDto data)
        {
            var entity = AutoMapperProfile.Map<ProductDto, Product>(data, true);
            _productUC.Update(entity);
            _eventTrace.AddTrace(Constants.EventCore.UpdateProduct, entity);
            return ApiResult.Success();
        }

        /// <summary>
        /// Delete a Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public ApiResult Remove(string id)
        {
            _productUC.Remove(id);
            _eventTrace.AddTrace(Constants.EventCore.DeleteProduct, id);
            return ApiResult.Success();
        }
    }
}
