using CaseWork.Data;
using CaseWork.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

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
        // if (await _dbContext.Companies.CountAsync(v => v.Name == company.Name) != 0)
        //     throw new Exception("Company name is busy!");
        
        var creator = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == accessEmail) ??
                      throw new Exception("Company owner not found!");

        company.UserCreator = creator;
        company.Users.Add(creator);
        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync();
        return company;
    }

    public async Task<IEnumerable<User>> GetAllMembers(int companyId, string accessEmail)
    {
        Company? company = await _dbContext.Companies
                .Include(v => v.Users)
                .SingleOrDefaultAsync(v => v.Id == companyId);
        
        if (company == null) throw new Exception("Company not found!");
        if (company.Users.Count(v => v.Email == accessEmail) == 0)
            throw new Exception("Access denied!");
        return company.Users.ToList();
    }

    public async Task<Company> GetUserCompany(string accessEmail)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == accessEmail);
        if (user == null) throw new Exception("User not found!");
        return await _dbContext.Companies
            .FirstOrDefaultAsync(v => v.Users.Contains(user)) 
            ?? throw new Exception("Company not found!");
    }
}