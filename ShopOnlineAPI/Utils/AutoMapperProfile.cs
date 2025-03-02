using AutoMapper;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Models;
using ShopOnlineCore.Entity;
using System.Text.Json;

namespace ShopOnlineAPI.Utils
{
    public class AutoMapperProfile : Profile
    {
        public static Target Map<Source, Target>(Source source, bool useProfile = false)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    var mappingExpresion = cfg.CreateMap<Source, Target>();
                    if (useProfile)
                        cfg.AddProfile(new AutoMapperProfile());
                });
                var mapper = config.CreateMapper();
                return mapper.Map<Source, Target>(source);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        /// <summary>
        /// Initializes a new instance of the AutoMapperProfile class and configures type mappings between various client and sale models.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the following mappings:
        /// <list type="bullet">
        /// <item>
        /// Maps <c>ClientModel</c> to <c>Client</c>.
        /// </item>
        /// <item>
        /// Configures bidirectional mappings for <c>Client</c> with <c>ClientDto</c> and <c>UserInfo</c>.
        /// </item>
        /// <item>
        /// Maps <c>Client</c> to <c>ClientModel</c> with custom member mappings:
        ///   - Generates a default UUID for <c>Id</c> if absent.
        ///   - Ignores <c>CreatedAt</c> and <c>Products</c>.
        ///   - Supports reverse mapping.
        /// </item>
        /// <item>
        /// Maps <c>Sale</c> to <c>SaleModel</c> with specific member configurations:
        ///   - Generates a default UUID for <c>Id</c> if needed.
        ///   - Maps <c>Client</c> to <c>ClientModelId</c> and <c>Product</c> to <c>ProductModelId</c>.
        ///   - Ignores <c>Client</c> and <c>Product</c>.
        ///   - Enables bidirectional mapping.
        /// </item>
        /// </list>
        /// </remarks>
        public AutoMapperProfile()
        {
            CreateMap<ClientModel, Client>();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Client, UserInfo>().ReverseMap();

            CreateMap<Client, ClientModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(dest => dest.Id ?? BaseEntity.CreateUUID()))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Sale, SaleModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(dest => dest.Id ?? BaseEntity.CreateUUID()))
               .ForMember(dest => dest.ClientModelId, opt => opt.MapFrom(src => src.Client))
               .ForMember(dest => dest.ProductModelId, opt => opt.MapFrom(src => src.Product))
               .ForMember(dest => dest.Client, opt => opt.Ignore())
               .ForMember(dest => dest.Product, opt => opt.Ignore())
               .ReverseMap();
        }
        public class JsonSerializeConvert : IValueConverter<object, string>
        {
            public string Convert(object source, ResolutionContext context)
            {
                return JsonSerializer.Serialize(source);
            }
        }

        public class JsonDeserializeConvert<T> : IValueConverter<string, T>
        {
            public T Convert(string source, ResolutionContext context)
            {
                return JsonSerializer.Deserialize<T>(source);
            }
        }
    }
}
