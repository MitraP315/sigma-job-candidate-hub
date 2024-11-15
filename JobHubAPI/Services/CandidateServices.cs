using JobHubAPI.Crud;
using JobHubAPI.Model;
using Microsoft.Extensions.Caching.Memory;

namespace JobHubAPI.Services
{
    public interface ICandidateServices
    {

        CrudService<Candidate> CandidateCrudService { get; set; }
        Task<ResponseModel> CreateOrUpdateCandidateInfo(Candidate candidate);
    }
    public class CandidateServices: ICandidateServices
    {



        private readonly IMemoryCache _cache;

        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);
        public CandidateServices(IMemoryCache cache)
        {
            _cache = cache;
        }
        public CrudService<Candidate> CandidateCrudService { get; set; } = new CrudService<Candidate>();
      
        public async Task<ResponseModel>CreateOrUpdateCandidateInfo(Candidate candidate)
        {
            try
            {
                string cacheKey = $"Candidate_{candidate.Email}";

                // Try getting the candidate data from cache
                if (!_cache.TryGetValue(cacheKey, out Candidate existingCandidate))
                {
                    // If data is not in cache, retrieve it from the database
                    existingCandidate = await CandidateCrudService.QueryAsync("select * from dbo.Candidates where Email=@Email", new { Email = candidate.Email });

                    // If found, cache it to reduce future database calls
                    if (existingCandidate != null)
                    {
                        _cache.Set(cacheKey, existingCandidate, _cacheExpiration);
                    }
                }

                if (existingCandidate == null)
                {
                    // Insert new candidate and cache the result
                    var newCandidate = await CandidateCrudService.InsertAsync(candidate);
                    candidate.Id = (int)newCandidate;

                    _cache.Set(cacheKey, candidate, _cacheExpiration);

                    return new ResponseModel(200, "Candidate Info Saved Successfully.", candidate.Email);
                }
                else
                {
                    // Candidate exists, update and refresh the cache
                    candidate.Id = existingCandidate.Id;
                    var updatedCandidate = await CandidateCrudService.UpdateAsync(candidate);

                    // Update cache with the latest data
                    _cache.Set(cacheKey, candidate, _cacheExpiration);

                    return new ResponseModel(200, "Candidate Info Updated Successfully.", updatedCandidate);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel(StatusCodes.Status500InternalServerError, ex.Message, candidate);
            }

        }
      
    }
}
