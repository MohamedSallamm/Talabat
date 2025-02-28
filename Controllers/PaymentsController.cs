using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entities;
//using Talabat.Core.Service;

namespace Talabat.API.Controllers
{
    //[Authorize]
    //public class PaymentsController : APIBaseController
    //{
    //  // private readonly IPaymentService _paymentService;
    //    private readonly IMapper _mapper;

    //    public PaymentsController(/*IPaymentService paymentService,*/ IMapper mapper)
    //    {
    //       //_paymentService = paymentService;
    //        _mapper = mapper;
    //    }

    //    //Create Or Update Endpoint
    //    [ProducesResponseType(typeof(CustomerBasketDTO),StatusCodes.Status200OK)]
    //    [ProducesResponseType(typeof(APIResponse),StatusCodes.Status400BadRequest)]
    //    [HttpPost]
    //    public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
    //    {
    //        var CustomerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
    //        if (CustomerBasket is null) return BadRequest(new APIResponse(400, "The Basket is Empty"));
    //        var Mapped = _mapper.Map<CustomerBasket, CustomerBasketDTO>(CustomerBasket);
    //        return Ok(Mapped);
    //    }

    //}
}
