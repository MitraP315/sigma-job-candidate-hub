using JobHubAPI.Crud;
using JobHubAPI.Model;

namespace JobHubAPI.Services
{
    public interface ICandidateServices
    {

        CrudService<Candidate> CandidateCrudService { get; set; }
        Task<ResponseModel> CreateOrUpdateCandidateInfo(Candidate candidate);
    }
    public class CandidateServices: ICandidateServices
    {

        public CrudService<Candidate> CandidateCrudService { get; set; } = new CrudService<Candidate>();
      
        public async Task<ResponseModel>CreateOrUpdateCandidateInfo(Candidate candidate)
        {
            try
            {
                Candidate existingCandidate = await CandidateCrudService.QueryAsync("select * from dbo.Candidates where Email=@email",new { @email = candidate.Email });
                if (existingCandidate == null)
                {
                    var newCandidate = await CandidateCrudService.InsertAsync(candidate);
                    return new ResponseModel(200, "Candidate Info Saved Successfully.", candidate.Email);
                }
                else
                {
                    candidate.Id = existingCandidate.Id;
                    var updateCandidate = await CandidateCrudService.UpdateAsync(candidate);
                    return new ResponseModel(200, "Candidate Info Updated Successfully.", updateCandidate);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel(StatusCodes.Status500InternalServerError, ex.Message, candidate);
            }

        }
      
    }
}
