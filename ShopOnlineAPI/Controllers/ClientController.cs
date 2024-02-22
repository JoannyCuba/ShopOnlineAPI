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
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventTrace _eventTrace;
        private readonly ClientUC _clientUC;

        public ClientController(IUnitOfWork unitOfWork, IEventTrace eventTrace)
        {
            _unitOfWork = unitOfWork;
            _eventTrace = eventTrace;
            _clientUC = new ClientUC(unitOfWork);
        }
        /// <summary>
		/// Returns the Client list.
		/// </summary>
		/// <param name="search"></param>
		/// <param name="page"></param>
		/// <param name="itemPerPage"></param>
		/// <returns></returns>
        [HttpGet]
        public ApiResult Get(string? search, int page = 1, int itemPerPage = 25)
        {
            var filter = new FilterDto() { search = search };
           
            var result = _clientUC.List(filter, page, itemPerPage);
            int total = _unitOfWork.Client.Count(filter);
            return ApiResult.Success(new
            {
                items = result.Select(x =>
                    AutoMapperProfile.Map<Client, ClientDto>(x, true)
                ).ToList(),
                total,
            });
        }

        /// <summary>
		/// Returns the Client by the Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        [HttpGet("{id}")]
        public ApiResult Get(string id)
        {
            var result = _clientUC.GetById(id);
            return ApiResult.Success(AutoMapperProfile.Map<Client, ClientDto>(result, true));
        }

        /// <summary>
        /// Create a Client.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ApiResult Add(ClientDto data)
        {
            var entity = AutoMapperProfile.Map<ClientDto, Client>(data, true);
            _clientUC.Add(entity);
            _eventTrace.AddTrace(Constants.EventCore.AddClient, entity);
            return ApiResult.Success();
        }

        /// <summary>
        /// Update a Client.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public ApiResult Update(ClientDto data)
        {
            var entity = AutoMapperProfile.Map<ClientDto, Client>(data, true);
            _clientUC.Update(entity);
            _eventTrace.AddTrace(Constants.EventCore.UpdateClient, entity);
            return ApiResult.Success();
        }

        /// <summary>
        /// Disable a Client.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public ApiResult Remove(string id)
        {
            _clientUC.Remove(id);
            _eventTrace.AddTrace(Constants.EventCore.DeleteClient, id);
            return ApiResult.Success();
        }
    }
}
