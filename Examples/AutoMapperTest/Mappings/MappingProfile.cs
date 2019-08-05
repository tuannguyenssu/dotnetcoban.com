using AutoMapper;
using AutoMapperTest.Models;

namespace AutoMapperTest.Mappings
{
    public class MappingProfile : Profile {
    public MappingProfile() {
        // Đưa hết các cấu hình bạn muốn map giữa các object vào đây
        // Thuộc tính FullName trong UserViewModel được kết hợp từ FirstName và LastName trong User
        CreateMap<User, UserViewModel>().ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName}   {s.LastName}"));
        CreateMap<UserViewModel, User>();
    }
}
}