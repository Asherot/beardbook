using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using BeardBook.Commands;
using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Models;
using BeardBook.Models.ConversationsViewModels;
using BeardBook.Models.HomeViewModels;
using BeardBook.Models.ProfileViewModels;

namespace BeardBook
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            var config = new MapperConfigurationExpression();

            config.CreateMap<User, OptionsBarViewModel>()
                .ForMember(
                    dest => dest.FullName,
                    opts => opts.MapFrom(
                        src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                    dest => dest.AvatarId,
                    opts => opts.MapFrom(
                        src => src.Avatar.Id));

            config.CreateMap<UserResult, UserViewModel>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(
                        src => src.User.Id))
                .ForMember(
                    dest => dest.Nickname,
                    opts => opts.MapFrom(
                        src => src.User.Nickname))
                .ForMember(
                    dest => dest.Email,
                    opts => opts.MapFrom(
                        src => src.User.Email))
                .ForMember(
                    dest => dest.FullName,
                    opts => opts.MapFrom(
                        src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(
                    dest => dest.AvatarId,
                    opts => opts.MapFrom(
                        src => src.User.Avatar.Id));

            config.CreateMap<User, ProfileViewModel>()
                .ForMember(
                    dest => dest.FullName,
                    opts => opts.MapFrom(
                        src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                    dest => dest.AvatarId,
                    opts => opts.MapFrom(
                        src => src.Avatar.Id));
            
            config.CreateMap<User, HomeViewModel>();

            config.CreateMap<User, EditViewModel>().ReverseMap();

            config.CreateMap<Post, PostViewModel>()
                .ForMember(
                    dest => dest.UserDisplayName,
                    opts => opts.MapFrom(
                        src => src.User.Nickname ?? $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(
                    dest => dest.AvatarId,
                    opts => opts.MapFrom(
                        src => src.User.Avatar.Id))
                .ForMember(
                    dest => dest.PhotosIds,
                    opts => opts.MapFrom(
                        src => src.MediaFiles
                            .Where(file => file.FileType == FileType.Photo)
                            .Select(file => file.Id)
                            .ToList()))
                .ForMember(
                    dest => dest.VideosIds,
                    opts => opts.MapFrom(
                        src => src.MediaFiles
                            .Where(file => file.FileType == FileType.Video)
                            .Select(file => file.Id)
                            .ToList()));

            config.CreateMap<File, UploadedMediaViewModel>()
                .ForMember(
                    dest => dest.FileId,
                    opts => opts.MapFrom(
                        src => src.Id));

            config.CreateMap<FileResult, UploadedMediaViewModel>()
                .ForMember(
                    dest => dest.FileId,
                    opts => opts.MapFrom(
                        src => src.File.Id))
                .ForMember(
                    dest => dest.Created,
                    opts => opts.MapFrom(
                        src => src.File.Created))
                .ForMember(
                    dest => dest.ThumbnailSrc,
                    opts => opts.MapFrom(
                        src => src.Thumbnail.ToBase64()));

            config.CreateMap<UpdateUserInfoCommand, User>();

            config.CreateMap<Conversation, ConversationViewModel>()
                .ForMember(
                    dest => dest.ConversationId,
                    opts => opts.MapFrom(
                        src => src.Id))
                .ForMember(
                    dest => dest.UserNames,
                    opts => opts.MapFrom(
                        src => src.Users
                            .Select(u => u.Nickname 
                            ?? $"{u.FirstName} {u.LastName}")
                            .ToArray()))
                .ForMember(
                    dest => dest.Messages,
                    opts => opts.MapFrom(
                        src => src.Messages
                            .Select(m => new MessageViewModel
                            {
                                Text = m.Text,
                                Created = m.Created,
                                UserId = m.UserId,
                                UserAvatarId = m.User.AvatarId ?? 0
                            })));

            Mapper.Initialize(config);
        }
    }
}