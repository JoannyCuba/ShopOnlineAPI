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
