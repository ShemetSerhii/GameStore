using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.DAL.DBContexts.MongoDB.MongoModel.EntityModel;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using System;
using System.Linq;

namespace GameStore.DAL.MappingProfile
{
    public class DalMappingProfile : Profile
    {
        public DalMappingProfile()
        {
            CreateMap<ProductMongo, Game>()
                .ForMember("Id", opt => opt.MapFrom(c => c.ProductID))
                .ForMember("CrossProperty", opt => opt.MapFrom(c => "M" + c.ProductID + "$$M$$" + c.CategoryID))
                .ForMember("PublisherId", opt => opt.MapFrom(c => c.SupplierID))
                .ForMember("Key", opt => opt.MapFrom(c => c.Key ?? "M" + c.ProductID))
                .ForMember("Name", opt => opt.MapFrom(c => c.ProductName))
                .ForMember("Description", opt => opt.MapFrom(c => c.QuantityPerUnit))
                .ForMember("Price", opt => opt.MapFrom(c => c.UnitPrice))
                .ForMember("UnitsInStock", opt => opt.MapFrom(c => c.UnitsInStock))
                .ForMember("DateAdded", opt => opt.MapFrom(c => DateTime.Parse("2019/01/02")))
                .ForMember("Publisher", opt => opt.Ignore())
                .ForMember("Genres", opt => opt.Ignore())
                .ForMember("PlatformTypes", opt => opt.Ignore());

            CreateMap<CustomerMongo, User>()
                .ForMember("Login", opt => opt.MapFrom(c => c.CustomerID))
                .ForMember("Name", opt => opt.MapFrom(c => c.ContactName))
                .ForMember("Address", opt => opt.MapFrom(c => c.City + "\n" + c.Address));

            CreateMap<Game, ProductMongo>()
                .ForMember("ProductID", opt => opt.MapFrom(c => c.Id))
                .ForMember("SupplierID", opt => opt.MapFrom(c => c.PublisherId))
                .ForMember("ProductName", opt => opt.MapFrom(c => c.Name))
                .ForMember("QuantityPerUnit", opt => opt.MapFrom(c => c.Description))
                .ForMember("UnitPrice", opt => opt.MapFrom(c => c.Price))
                .ForMember("UnitsInStock", opt => opt.MapFrom(c => c.UnitsInStock))
                .ForMember("Discontinued", opt => opt.MapFrom(c => c.Discontinued))
                .ForMember("Publisher", opt => opt.MapFrom(c => c.Publisher.CompanyName))
                .ForMember("Genres", opt => opt.MapFrom(c => c.Genres.Select(g => g.Name)))
                .ForMember("PlatformTypes", opt => opt.MapFrom(c => c.PlatformTypes.Select(p => p.Type)));

            CreateMap<OrderMongo, Order>()
                .ForMember("Id", opt => opt.MapFrom(c => c.OrderID))
                .ForMember("CrossId", opt => opt.MapFrom(c => c.Id.ToString()))
                .ForMember("OrderDate", opt => opt.MapFrom(c => DateTime.Parse(c.OrderDate)))
                .ForMember("OrderStatus", opt => opt.MapFrom(c => "Paid"))
                .ForMember("ShippedDate", opt => opt.MapFrom(c => c.ShippedDate != "NULL" ? DateTime.Parse(c.ShippedDate) : DateTime.MinValue))
                .ForMember("CustomerId", opt => opt.MapFrom(c => c.CustomerID));

            CreateMap<OrderDetailMongo, OrderDetail>()
                .ForMember("GameId", opt => opt.MapFrom(c => c.ProductID))
                .ForMember("Price", opt => opt.MapFrom(c => c.UnitPrice))
                .ForMember("Quantity", opt => opt.MapFrom(c => c.Quantity))
                .ForMember("Discount", opt => opt.MapFrom(c => float.Parse(c.Discount.ToString())))
                .ForMember("OrderId", opt => opt.MapFrom(c => c.OrderID));

            CreateMap<SupplierMongo, Publisher>()
                .ForMember("Id", opt => opt.MapFrom(c => c.SupplierID))
                .ForMember("CrossProperty", opt => opt.MapFrom(c => "M" + c.SupplierID))
                .ForMember("CompanyName", opt => opt.MapFrom(c => c.CompanyName))
                .ForMember("HomePage", opt => opt.MapFrom(c => c.HomePage))
                .ForMember("Description",
                    opt => opt.MapFrom(c => c.City + "\n." + c.Address + "\n." + c.ContactName + "\n." + c.Phone));

            CreateMap<ShipperMongo, Shipper>()
                .ForMember("Id", opt => opt.MapFrom(c => c.ShipperID));

            CreateMap<Publisher, SupplierMongo>()
                .ForMember("SupplierID", opt => opt.MapFrom(c => int.Parse(c.CrossProperty.Substring(1))))
                .ForMember("City", opt => opt.MapFrom(
                    c => c.Description.Split('.').Length > 0 ? c.Description.Split('.')[0] : ""))
                .ForMember("Address", opt => opt.MapFrom(
                    c => c.Description.Split('.').Length > 1 ? c.Description.Split('.')[1] : ""))
                .ForMember("ContactName", opt => opt.MapFrom(
                    c => c.Description.Split('.').Length > 2 ? c.Description.Split('.')[2] : ""))
                .ForMember("Phone", opt => opt.MapFrom(
                    c => c.Description.Split('.').Length > 3 ? c.Description.Split('.')[3] : ""));

            CreateMap<CategorieMongo, Genre>()
                .ForMember("Name", opt => opt.MapFrom(c => c.CategoryName))
                .ForMember("Parent", opt => opt.Ignore())
                .ForMember("CrossProperty", opt => opt.MapFrom(c => "M" + c.CategoryID));

            CreateMap<Genre, CategorieMongo>()
                .ForMember("CategoryID", opt => opt.MapFrom(c => c.CrossProperty.Substring(1)))
                .ForMember("CategoryName", opt => opt.MapFrom(c => c.Name))
                .ForMember("Parent", opt => opt.MapFrom(c => c.Parent != null ? c.Parent.Name : ""))
                .ForMember("SqlGamesId", opt => opt.MapFrom(c => c.Games.Select(g => g.Id)));       
        }

        public override string ProfileName
        {
            get { return this.GetType().ToString(); }
        }
    }
}
