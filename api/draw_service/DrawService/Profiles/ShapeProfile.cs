using AutoMapper;
using BoardService;
using DrawService.Dtos;
using DrawService.Models;
using Newtonsoft.Json;
using static DrawService.Helpers.Constant;

namespace DrawService.Profiles
{
    public class ShapeProfile : Profile
    {
        public ShapeProfile()
        {
            CreateMap<ShapeReadDto, ShapeGrpc>().ForMember(dest => dest.Data, opt => opt.MapFrom(src => SerializeShapeDataToJson(src.ClassName, (object)src.Data)));
            CreateMap<ShapeGrpc, ShapeReadDto>().ForMember(dest => dest.Data, opt => opt.MapFrom(src => DeserializeShapeDataFromJson(src.Data, src.ClassName)));
        }

        public string? SerializeShapeDataToJson(string dataType, object data)
        {
            var jsonData = Convert.ToString((dynamic)data);

            if (dataType == ShapeDataType.LinePath)
            {
                return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<PathData>(jsonData));
            }
            else if (dataType == ShapeDataType.ErasedLinePath)
            {
                return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<PathData>(jsonData));
            }
            else if (dataType == ShapeDataType.Text)
            {
                return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<TextData>(jsonData));
            }

            return null;
        }

        public object? DeserializeShapeDataFromJson(string dataType, string value)
        {
            if (dataType == ShapeDataType.LinePath)
            {
                return JsonConvert.DeserializeObject<PathData>(value);
            }
            else if (dataType == ShapeDataType.ErasedLinePath)
            {
                return JsonConvert.DeserializeObject<PathData>(value);
            }
            else if (dataType == ShapeDataType.Text)
            {
                return JsonConvert.DeserializeObject<TextData>(value);
            }
            else
            {
                return null;
            }
        }
    }
}