using CaseWork.Data;
using CaseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseWork.Services.Companies;

public class CompaniesService : ICompaniesService
{
    private readonly CaseWorkContext _dbContext;

    public CompaniesService(CaseWorkContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Company> Create(Company company, string accessEmail)
    {
        if (await _dbContext.Companies.CountAsync(v => v.Name == company.Name) != 0)
            throw new Exception("Company name is busy!");
        
        var creator = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == accessEmail) ??
                      throw new Exception("Company owner not found!");

        company.UserCreator = creator;
        company.Users.Add(creator);
        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync();
        return company;
    }
}