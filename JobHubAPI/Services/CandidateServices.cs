using JobHubAPI.Crud;
using JobHubAPI.Model;

namespace JobHubAPI.Services
{
    public interface ICandidateServices
    {

        CrudService<Candidate> CandidateCrudService { get; set; }
    }
    public class CandidateServices: ICandidateServices
    {

        public CrudService<Candidate> CandidateCrudService { get; set; } = new CrudService<Candidate>();
    }
}
