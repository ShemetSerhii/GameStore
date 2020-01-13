using System;
using AutoMapper;
using GameStore.BLL.FilterPipeline.Model;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Models;
using GameStore.WEB.Models.DomainViewModel.EditorModels;
using GameStore.WEB.Models.DomainViewModel.Identity;
using GameStore.WEB.Models.FilterModel;
using GameStore.WEB.Models.IdentityModel;
using System.Linq;

namespace GameStore.WEB.AutoMapper
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<Game, GameViewModel>()
                .ForMember(game => game.NameRU, opt => opt.MapFrom(src => src.GameTranslates.SingleOrDefault(x => x.Language == "ru-RU").Name))
                .ForMember(game => game.DescriptionRU, opt => opt.MapFrom(src => src.GameTranslates.SingleOrDefault(x => x.Language == "ru-RU").Description))
                .ForMember(game => game.DatePublication, opt => opt.MapFrom(src => src.DatePublication.ToShortDateString()))
                .ForMember(game => game.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => c)))
                .ForMember(game => game.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g)))
                .ForMember(game => game.PlatformTypes, opt => opt.MapFrom(src => src.PlatformTypes.Select(p => p)));


            CreateMap<CommentViewModel, Comment>()
                .ForMember(comment => comment.Game, opt => opt.Ignore())
                .ForMember(comment => comment.ParentComment, opt => opt.Ignore());

            CreateMap<GameViewModel, Game>()
                .ForMember(game => game.DatePublication, opt => opt.MapFrom(src => DateTime.Parse(src.DatePublication)))
                .ForMember(game => game.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => c)))
                .ForMember(game => game.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g)))
                .ForMember(game => game.PlatformTypes, opt => opt.MapFrom(src => src.PlatformTypes.Select(p => p)));

            CreateMap<Comment, CommentViewModel>();

            CreateMap<Publisher, PublisherViewModel>();

            CreateMap<PublisherViewModel, Publisher>();

            CreateMap<Genre, GenreViewModel>()
                .ForMember(genre => genre.NameTranslate, opt => opt.MapFrom(src => src.Name))
                .ForMember(genre => genre.NameRu, opt => opt.MapFrom(src => src.GenreTranslates.SingleOrDefault(x => x.Language == "ru-RU").Name));

            CreateMap<GenreViewModel, Genre>();

            CreateMap<PlatformType, PlatformTypeViewModel>();

            CreateMap<PlatformTypeViewModel, PlatformType>();

            CreateMap<Order, OrderViewModel>();

            CreateMap<Order, OrderEditModel>();

            CreateMap<OrderEditModel, Order>();

            CreateMap<OrderDetail, OrderDetailViewModel>();

            CreateMap<FilterViewModel, FilterModelDTO>();

            CreateMap<RegisterViewModel, User>()
                .ForMember(user => user.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(user => user.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(user => user.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(user => user.Password, opt => opt.MapFrom(src => src.Password.GetHashCode()));

            CreateMap<User, UserViewModel>();

            CreateMap<User, UserEditModel>();

            CreateMap<UserEditModel, User>();
        }
    }
}