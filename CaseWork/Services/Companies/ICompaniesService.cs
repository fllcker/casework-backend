using CaseWork.Models;

namespace CaseWork.Services.Companies;

public interface ICompaniesService
{
    public Task<Company> Create(Company company, string accessEmail);
}