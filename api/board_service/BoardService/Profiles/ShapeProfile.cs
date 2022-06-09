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