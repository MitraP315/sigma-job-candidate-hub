using JobHubAPI.Crud;
using JobHubAPI.Model;
using JobHubAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace JobHubAPI.Controllers
{
    [Route("api/candidate")]

    [ApiExplorerSettings(GroupName = "JobCandidateHub")]
    public class CandidateController:ControllerBase
    {
        private readonly ICandidateServices _candidateServices;
        public CandidateController(ICandidateServices candidateServices)
        {
            _candidateServices = candidateServices;
        }
        protected string GetModelErrors(ModelStateDictionary modelState)
        {
            return string.Join("; ", modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        }



        [HttpPost]
        public async Task<ResponseModel> Save(CandidateViewModel dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResponseModel(500, "success", GetModelErrors(ModelState));
                }
                dto.Email=dto.Email.Trim();
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Email pattern requiring @ and . after it

                if (!Regex.IsMatch(dto.Email, pattern))
                {
                    return new ResponseModel(400, "Email is invalid. Email should contain @/.", dto);
                }
                    Candidate  candidate= new Candidate
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    PreferredContactTime = dto.PreferredContactTime,
                    LinkedInProfile = dto.LinkedInProfile,
                    GitHubProfile = dto.GitHubProfile,
                    Comment = dto.Comment
                };
                var data = await _candidateServices.CreateOrUpdateCandidateInfo(candidate);
                return data;
            }
            catch (Exception ex)
            {
                return new ResponseModel(StatusCodes.Status500InternalServerError, ex.Message, dto);
            }
        }
    }
}
