using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Shortener.Data;
using Shortener.Controllers;
using Shortener.Models;
using Microsoft.AspNetCore.Mvc;
using Shortener.Service;

namespace Shortener.Tests
{
  public class Shorteners
  {
    private readonly Mock<IShortenersRepository> _shortenerRepository;
    private readonly ShortenersRepository _repository;
    private readonly URLShortenersController _controller;
    public Shorteners()
    {
        _shortenerRepository = new Mock<IShortenersRepository>();
        _repository = new ShortenersRepository(null);
        _controller = new URLShortenersController(_shortenerRepository.Object);
    }
    [Fact]
    public void IsValidHttpUrl_ReturnsTrue()
    {
        const string longUrl="https://www.google.com";
        var isValid = _repository.ValidateUrl(longUrl);

        Assert.True(isValid);
    }
    [Fact]
    public void IsValidHttpUrl_ReturnsFalse()
    {
        const string longUrl="www.google.com";
        var isValid = _repository.ValidateUrl(longUrl);

        Assert.False(isValid);
    }
    [Fact]
    public void GenerateShortUrl_ReturnsValidShortUrl()
    {
      var shortUrl = _repository.GenerateShortUrl();

      Assert.Equal(7, shortUrl.Length);
      Assert.Matches("^[A-Za-z0-9]{7}$", shortUrl);
    }
    [Fact]
    public async Task CreateShortUrl_Returns_ShortUrl()
    {
        const string longUrl="https://www.google.com";
        const string shortUrl = "tx.nano/9eXyS77";
        _shortenerRepository.Setup(x => x.GetShortUrl(longUrl)).ReturnsAsync(shortUrl);

        var response = await _controller.CreateShortUrl(longUrl);

        var okResult = Assert.IsType<OkObjectResult>(response);
        var resultValue = Assert.IsType<string>(okResult.Value);

        Assert.Equal(shortUrl, resultValue);
    }
  }
}