using System.Collections.Generic;
using AutoMapper;
using AutoMapperTest.Models;

namespace AutoMapperTest.Mappings
{
    public class AppMapperConfiguration
    {
        public static List<Profile> RegisterMappings()
        {
            var cfg = new List<Profile>
            {
                // Thêm các MappingProfile khác vào đây
                new MappingProfile()
            };

            return cfg;
        }
    }
    public class MappingProfile : Profile {
    public MappingProfile() {
        // Đưa hết các cấu hình bạn muốn map giữa các object vào đây
        // Thuộc tính FullName trong UserViewModel được kết hợp từ FirstName và LastName trong User
        CreateMap<User, UserViewModel>().ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName}   {s.LastName}"));
        CreateMap<UserViewModel, User>();
    }
}
}