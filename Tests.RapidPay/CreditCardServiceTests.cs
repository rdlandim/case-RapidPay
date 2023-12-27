using API.RapidPay.Controllers;
using Core.RapidPay.Services.CreditCards;
using DAL.RapidPay.DTO.CreditCards;
using Interfaces.RapidPay.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace Tests.RapidPay;

[TestFixture]
public class CreditCardService_ShouldCreate
{
    private Mock<ICreditCardService> _creditCardServiceMock;
    private Mock<IPaymentHandler> _paymentHandlerMock;
    private Mock<CreateCreditCardRequest> _createCreditCardRequestMock;
    private Mock<CreateCreditCardResponse> _createCreditCardResponseMock;
    private CreditCardController _controller;
    private Mock<CreditCardBalanceRequest> _creditCardBalanceRequest;
    private Mock<CreditCardBalanceResponse> _creditCardBalanceResponse;
    private Mock<CreditCardPaymentRequest> _creditCardPaymentRequest;
    private Mock<PaymentResponse> _creditCardPaymentResponse;

    [SetUp]
    public void Setup()
    {
        _creditCardServiceMock = new Mock<ICreditCardService>();
        _paymentHandlerMock = new Mock<IPaymentHandler>();

        MockRequests();
        MockResponses();

        _controller = new CreditCardController(_creditCardServiceMock.Object, _paymentHandlerMock.Object);
    }

    private void MockRequests()
    {
        MockCreateRequest();
        MockGetBalanceRequest();
        MockPaymentRequest();
    }

    private void MockPaymentRequest()
    {
        _creditCardPaymentRequest = new Mock<CreditCardPaymentRequest>();

        _creditCardPaymentRequest.Setup(cc => cc.UserId).Returns(1);
        _creditCardPaymentRequest.Setup(cc => cc.CreditCardNumber).Returns("123456789101112");
        _creditCardPaymentRequest.Setup(cc => cc.CVC).Returns(123);
        _creditCardPaymentRequest.Setup(cc => cc.ValidUntil).Returns("2028-07");
        _creditCardPaymentRequest.Setup(cc => cc.Value).Returns(new decimal(521.98));
    }

    private void MockGetBalanceRequest()
    {
        _creditCardBalanceRequest = new Mock<CreditCardBalanceRequest>();
        _creditCardBalanceRequest.Setup(cc => cc.Id).Returns(1);
    }

    private void MockCreateRequest()
    {
        _createCreditCardRequestMock = new Mock<CreateCreditCardRequest>();
        _createCreditCardRequestMock.Setup(cc => cc.UserId).Returns(1);
    }

    private void MockResponses()
    {
        MockCreateResponse();
        MockGetBalanceResponse();
        MockPaymentResponse();
    }

    private void MockPaymentResponse()
    {
        _creditCardPaymentResponse = new Mock<PaymentResponse>();

        _creditCardPaymentResponse.Setup(cc => cc.GUID).Returns(Guid.NewGuid().ToString());
        _creditCardPaymentResponse.Setup(cc => cc.Success).Returns(true);
        _creditCardPaymentResponse.Setup(cc => cc.Message).Returns("Payment Successful");
        _creditCardPaymentResponse.Setup(cc => cc.Balance).Returns(_creditCardBalanceResponse.Object);
    }

    private void MockGetBalanceResponse()
    {
        _creditCardBalanceResponse = new Mock<CreditCardBalanceResponse>();
    }

    private void MockCreateResponse()
    {
        _createCreditCardResponseMock = new Mock<CreateCreditCardResponse>();

        _createCreditCardResponseMock.Setup(cc => cc.CVC).Returns(123);
        _createCreditCardResponseMock.Setup(cc => cc.ValidUntil).Returns("2028-07");
        _createCreditCardResponseMock.Setup(cc => cc.Number).Returns("123456789101112");
        _createCreditCardResponseMock.Setup(cc => cc.Id).Returns(1);
    }

    [Test]
    public void CreateCreditCard_ShouldReturn_Http200()
    {
        _creditCardServiceMock.Setup(cc => cc.CreateCard(_createCreditCardRequestMock.Object)).Returns(_createCreditCardResponseMock.Object);

        var result = _controller.CreateCard(_createCreditCardRequestMock.Object) as ObjectResult;

        _creditCardServiceMock.Verify(cc => cc.CreateCard(_createCreditCardRequestMock.Object), Times.Once);

        var response = result.Value as CreateCreditCardResponse;

        Assert.That(HttpStatusCode.OK, Is.EqualTo((HttpStatusCode)result.StatusCode));
    }

    [Test]
    public void GetCreditCardBalance_ShouldReturn_ZeroBalance()
    {
        _creditCardServiceMock.Setup(cc => cc.GetBalance(_creditCardBalanceRequest.Object)).Returns(_creditCardBalanceResponse.Object);

        var result = _controller.GetBalance(_creditCardBalanceRequest.Object) as ObjectResult;

        _creditCardServiceMock.Verify(cc => cc.GetBalance(_creditCardBalanceRequest.Object), Times.Once);

        var response = result.Value as CreditCardBalanceResponse;

        Assert.That(response.Balance, Is.Zero);
    }

    [Test]
    public void MakePayment_ShouldReturn_UpdatedBalance()
    {
        _creditCardBalanceResponse.Setup(cc => cc.PreviousBalance).Returns(0);
        _creditCardBalanceResponse.Setup(cc => cc.Balance).Returns(_creditCardPaymentRequest.Object.Value + _creditCardBalanceResponse.Object.PreviousBalance);

        _paymentHandlerMock.Setup(p => p.Handle(_creditCardPaymentRequest.Object)).Returns(_creditCardPaymentResponse.Object);
        _creditCardServiceMock.Setup(cc => cc.Pay(_creditCardPaymentRequest.Object)).Returns(_creditCardBalanceResponse.Object);

        var result = _controller.Pay(_creditCardPaymentRequest.Object) as ObjectResult;

        _paymentHandlerMock.Verify(p => p.Handle(_creditCardPaymentRequest.Object), Times.AtLeastOnce);

        var response = result.Value as PaymentResponse;

        Assert.That(response.Balance.Balance, Is.EqualTo(_creditCardPaymentRequest.Object.Value + response.Balance.PreviousBalance));
    }
}