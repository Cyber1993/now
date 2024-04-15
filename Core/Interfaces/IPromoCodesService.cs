using Core.Models.Promocode;
using Core.Models.PromoCode;
using Core.Services;

namespace Core.Interfaces
{
    public interface IPromoCodesService
    {
        Task<IEnumerable<GetPromoCodeDto>> GetAll();
        Task<GetPromoCodeDto> GetById(int id);
        Task Create(CreatePromoCodeDto promoCodeDto);
        Task<string> Activate(string promoCode, string userId);
        Task Update(int id, EditPromoCodeDto promoCodeDto);
        Task Delete(int id);
    }
}
