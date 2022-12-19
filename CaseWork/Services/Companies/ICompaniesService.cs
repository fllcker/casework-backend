using CaseWork.Models;

namespace CaseWork.Services.Companies;

public interface ICompaniesService
{
    public Task<Company> Create(Company company, string accessEmail);
    public Task<IEnumerable<User>> GetAllMembers(int companyId, string accessEmail);
    public Task<Company> GetUserCompany(string accessEmail);
}