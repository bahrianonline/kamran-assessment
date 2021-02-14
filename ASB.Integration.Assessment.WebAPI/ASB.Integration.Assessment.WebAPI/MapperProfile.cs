using System;
using AutoMapper;

namespace ASB.Integration.Assessment.WebAPI
{
    /// <summary>
    /// Mapper profile.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfile"/> class.
        /// </summary>
        public MapperProfile()
        {
            CreateMap<DatabaseContext.EntityModels.CreditCardEntity, Models.CreditCardModel>()
                .ForMember(model => model.CardHolderName, act => act.MapFrom(entity => entity.Name))
                .ForMember(model => model.CardStoreId, act => act.MapFrom(entity => entity.Id))
                .ForMember(model => model.CardNumber, act => act.MapFrom(entity => entity.CardNumber))
                .ForMember(model => model.Cvc, act => act.MapFrom(entity => entity.Cvc))
                .ForMember(model => model.CardExpiryDate, act => act.MapFrom(entity => entity.CardExpiryDate))
                .ForMember(model => model.CreatedAt, act => act.MapFrom(entity => entity.CreatedAt));

            CreateMap<Models.CreditCardModel, DatabaseContext.EntityModels.CreditCardEntity>()
                .ForMember(model => model.Name, act => act.MapFrom(model => model.CardHolderName))
                .ForMember(entity => entity.CardNumber, act => act.MapFrom(model => model.CardNumber))
                .ForMember(entity => entity.Cvc, act => act.MapFrom(model => model.Cvc))
                .ForMember(entity => entity.CardExpiryDate, act => act.MapFrom(model => model.CardExpiryDate))
                .ForMember(entity => entity.CreatedAt, act => act.MapFrom(model => DateTime.Now));
        }
    }
}
