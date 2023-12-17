using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Reporitories.Contract;

namespace Talabat.APIs.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustBasket(string basketId)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);

            return Ok(basket ?? new CustomerBasket(basketId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> SetCustBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var resBasket = await _basketRepo.SetBasketAsync(mappedBasket);
            return (resBasket is null) ? BadRequest(new ApiResponse(400)) : Ok(resBasket);
        }

        [HttpDelete]
        public async Task DeleteCustBasket(string basketId)
        {
            await _basketRepo.DeleteBasketAsync(basketId);
        }

    }
}
