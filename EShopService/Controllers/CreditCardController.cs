using Microsoft.AspNetCore.Mvc;
using EShop.Domain.Exceptions;
using EShop.Application;

namespace EShopService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardController : ControllerBase
{
    [HttpGet("validate")]
    public IActionResult Validate(string cardNumber)
    {
        var service = new CreditCardService();

        try
        {
            bool isValidLength = service.ValidateLength(cardNumber);
            if (!isValidLength)
            {
                return BadRequest("Card number is invalid.");
            }
        }
        catch (CardNumberTooLongException ex)
        {
            return StatusCode(414, ex.Message);
        }
        catch (CardNumberTooShortException ex)
        {
            return BadRequest(ex.Message);
        }

        if (!service.ValidateCard(cardNumber))
        {
            return BadRequest("Card number is invalid.");
        }

        string issuer;
        try
        {
            issuer = service.GetCardType(cardNumber);
        }
        catch (CardNumberInvalidException ex)
        {
            return StatusCode(406, ex.Message);
        }

        return Ok(new { status = "valid", issuer });
    }
}
