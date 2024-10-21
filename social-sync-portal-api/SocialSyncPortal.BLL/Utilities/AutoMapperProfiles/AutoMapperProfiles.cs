using SocialSyncPortal.DAL.Entities;
using AutoMapper;
using SocialSyncPortal.DTO.DTOs.User;
using SocialSyncPortal.DTO.DTOs.SocialPost;

namespace SocialSyncPortal.BLL.Utilities.AutoMapperProfiles;

public static class AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserToAddDTO>().ReverseMap();
            CreateMap<User, UserToUpdateDTO>().ReverseMap();
            CreateMap<User, UserToRegisterDTO>().ReverseMap();
            CreateMap<User, UserToReturnDTO>().ReverseMap();

            CreateMap<SocialPost, SocialPostDTO>().ReverseMap();
            CreateMap<SocialPost, SocialPostToAddDTO>().ReverseMap();
            CreateMap<SocialPost, SocialPostToUpdateDTO>().ReverseMap();
        }
    }
}
