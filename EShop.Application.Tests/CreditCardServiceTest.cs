using System.ComponentModel.DataAnnotations;
using Xunit;

namespace EShop.Application.Tests;

public class CreditCardServiceTest
{

    [Fact]
    public void ValidateCard_NumberTooShort_ReturnsFalse()
    {
        var service = new CreditCardService();
        string cardNumber = "123456789012";

        bool isValid = service.ValidateLength(cardNumber);

        Assert.False(isValid);
    }

    [Fact]
    public void ValidateCard_NumberTooLong_ReturnsFalse()
    {
        var service = new CreditCardService();
        string cardNumber = "12345678901234567890";

        bool isValid = service.ValidateLength(cardNumber);

        Assert.False(isValid);
    }
    [Fact]
    public void CreditCardService_Perfect_ReturnsTrue()
    {
        var service = new CreditCardService();
        string cardNumber = "123456789012345";

        bool isValid = service.ValidateLength(cardNumber);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("4024007165401778", true)]
    [InlineData("4024 0071 6540 1778", true)]
    [InlineData("4024-0071-6540-1778", true)]
    [InlineData("4024%0071%6540%1778", false)]
    public void ValidateCard_AllowsDifferentFormatting(string cardNumber, bool expected)
    {
        var service = new CreditCardService();

        bool isValid = service.ValidateLength(cardNumber);

        Assert.Equal(expected, isValid);
    }

    [Theory]
    [InlineData("4111 1111 1111 1112", false)]
    [InlineData("4111-1111-1111-1110", false)]
    [InlineData("4111111111111112", false)]
    [InlineData("4111 1111 1111 1111", true)]
    public void ValidateCard_ReturnsExpectedResult(string cardNumber, bool expected)
    {
        var service = new CreditCardService();

        bool result = service.ValidateCard(cardNumber);

        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetCardType_Visa_ReturnsVisa()
    {
        var service = new CreditCardService();
        string cardNumber = "4024-0071-6540-1778";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("Visa", type);
    }

    [Fact]
    public void GetCardType_MasterCard_ReturnsMasterCard()
    {
        var service = new CreditCardService();
        string cardNumber = "5530016454538418";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("MasterCard", type);
    }

    [Fact]
    public void GetCardType_AmericanExpress_ReturnsAmericanExpress()
    {
        var service = new CreditCardService();
        string cardNumber = "378523393817437";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("American Express", type);
    }

    [Fact]
    public void GetCardType_Discover_ReturnsDiscover()
    {
        var service = new CreditCardService();
        string cardNumber = "6011000990139424";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("Discover", type);
    }

    [Fact]
    public void GetCardType_JCB_ReturnsJCB()
    {
        var service = new CreditCardService();
        string cardNumber = "3528000000000000";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("JCB", type);
    }

    [Fact]
    public void GetCardType_DinersClub_ReturnsDinersClub()
    {
        var service = new CreditCardService();
        string cardNumber = "30000000000000";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("Diners Club", type);
    }

    [Fact]
    public void GetCardType_Maestro_ReturnsMaestro()
    {
        var service = new CreditCardService();
        string cardNumber = "6759649826438453";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("Maestro", type);
    }

    [Fact]
    public void GetCardType_UnknownCardType_ReturnsUnknown()
    {
        var service = new CreditCardService();
        string cardNumber = "1234567890123456";

        string type = service.GetCardType(cardNumber);

        Assert.Equal("Unknown", type);
    }
}
