using AutoMapper;
using BoardService.Models;
using Newtonsoft.Json;
using static BoardService.Helpers.Constant;

namespace BoardService.Profiles
{
    public class ShapeProfile : Profile
    {
        public ShapeProfile()
        {
            CreateMap<ShapeGrpc, Shape>().ForMember(dest => dest.Data, opt => opt.MapFrom(src => DeserializeShapeDataFromJson(src.ClassName, src.Data)));
            CreateMap<Shape, ShapeGrpc>().ForMember(dest => dest.Data, opt => opt.MapFrom(src => SerializeShapeDataToJson(src.ClassName, (object)src.Data)));
        }

        public string? SerializeShapeDataToJson(string dataType, object data)
        {
            // var jsonData = Convert.ToString((dynamic)data);

            if (dataType == ShapeDataType.LinePath)
            {
                return JsonConvert.SerializeObject(data);
            }
            else if (dataType == ShapeDataType.ErasedLinePath)
            {
                return JsonConvert.SerializeObject(data);
            }
            else if (dataType == ShapeDataType.Text)
            {
                return JsonConvert.SerializeObject(data);
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