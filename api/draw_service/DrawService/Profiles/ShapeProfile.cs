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
            CreateMap<ShapeReadDto, ShapeGrpc>().ForMember(dest => dest.Id, opt => opt.Ignore())
                                                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => SerializeShapeDataToJson(src.ClassName, (object)src.Data)));
            CreateMap<ShapeGrpc, ShapeReadDto>().ForMember(dest => dest.Data, opt => opt.MapFrom(src => DeserializeShapeDataFromJson(src.ClassName, src.Data)));
        }

        public string? SerializeShapeDataToJson(string dataType, object data)
        {
            // Get type of data instance
            var type = data.GetType();
            dynamic jsonData;

            if (type.FullName == "System.Text.Json.JsonElement")
            {
                // Dynamic object
                var temp = Convert.ToString((dynamic)data);
                jsonData = JsonConvert.DeserializeObject<PathData>(temp);
            }
            else
            {
                // Have Type object
                jsonData = data;
            }

            // Convert to Json
            if (dataType == ShapeDataType.LinePath)
            {
                return JsonConvert.SerializeObject(jsonData);
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