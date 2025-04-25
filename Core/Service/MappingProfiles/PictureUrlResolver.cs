using AutoMapper;
using DomainLayer.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects;

namespace Service.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDto, string>
	{

		//https://localhost:7277/{src,PictureUrl}
		public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
		{
			if (string.IsNullOrEmpty(source.PictureUrl))
				return string.Empty;
			else
			{
				//var url = $"https://localhost:7277/{source.PictureUrl}";
				var url = $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
				return url;
			}
		}
	}
}
