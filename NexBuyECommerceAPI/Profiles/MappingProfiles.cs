using AutoMapper;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Models;

namespace NexBuyECommerceAPI.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            //CreateMap<EditProfileViewModel, Cashier>()
            // .ForMember(Cashier => Cashier.ApplicationUser, act => act.Ignore())
            // .ForMember(Cashier => Cashier.ApplicationUserId, act => act.Ignore());

            //CreateMap<EditProfileViewModel, StoreManager>()
            //    .ForMember(Cashier => Cashier.ApplicationUser, act => act.Ignore())
            //    .ForMember(Cashier => Cashier.ApplicationUserId, act => act.Ignore());

            //CreateMap<UpdateUserRoleViewModel, MockViewModel>();


            //CreateMap<SupplierViewModel, Supplier>()
            //   .ForMember(s => s.Status, s => s.Ignore());

            //CreateMap<Supplier, SupplierViewModel>();

            CreateMap<ProductViewModel, Product>().ReverseMap();


            CreateMap<ProductCategoryViewModel, ProductCategory>().ReverseMap();

            CreateMap<EditCategoryViewModel, ProductCategory>().ReverseMap();


            CreateMap<OrderViewModel, Order>()
                 .ForMember(order => order.OrderItems, act => act.Ignore())
                 .ForMember(order => order.Price, act => act.Ignore());

            CreateMap<Report, ReportViewModel>()
                .ForMember(model => model.Orders, act => act.MapFrom(report => report.Orders))
                .ForMember(model => model.ReportProducts, act => act.MapFrom(report => report.ReportProducts))
                .ForMember(model => model.TotalRevenueForReport, act => act.MapFrom(report => report.TotalRevenueForReport));

            //CreateMap<TransactionViewModel, TransactionDetails>()
            //    .ForMember(details => details.amount, act => act.Ignore())
            //    .ForMember(details => details.txRef, act => act.Ignore())
            //    .ForMember(details => details.PBFPubKey, act => act.Ignore())
            //    .ForMember(details => details.cvv, act => act.MapFrom(model => model.Cvv))
            //    .ForMember(details => details.cardno, act => act.MapFrom(model => model.CardNumber))
            //    .ForMember(details => details.email, act => act.MapFrom(model => model.Email))
            //    .ForMember(details => details.expirymonth, act => act.MapFrom(model => model.ExpiryMonth))
            //    .ForMember(details => details.expiryyear, act => act.MapFrom(model => model.ExpiryYear));

        }
    }
}
