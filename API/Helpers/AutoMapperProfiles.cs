using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using System;
using System.Linq;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(
                    dest => dest.PhotoUrl,
                    opt => opt.MapFrom(
                        src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(
                        src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>()
                .ForMember(u => u.Create, opt => opt.MapFrom(c => DateTime.UtcNow));
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src =>
                    src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src =>
                    src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Photo, PhotoForApprovalDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src =>
                    src.AppUser.UserName));
        }
    }
}
