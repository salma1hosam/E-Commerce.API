using DomainLayer.Contracts;
using ServiceAbstraction;
using System.Text.Json;

namespace Service
{
	internal class CacheService(ICacheRepository _cacheRepository) : ICacheService
	{
		public async Task<string?> GetAsync(string cacheKey) => await _cacheRepository.GetAsync(cacheKey);

		public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
		{
			var value = JsonSerializer.Serialize(cacheValue);
			await _cacheRepository.SetAsync(cacheKey, value, timeToLive);
		}
	}
}
