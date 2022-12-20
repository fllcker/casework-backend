using System.Net;
using CaseWork.Data;
using CaseWork.Exceptions;
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
        var creator = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == accessEmail) 
                      ?? throw new ErrorResponse("Company owner not found!", HttpStatusCode.NotFound);

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
        
        if (company == null) 
            throw new ErrorResponse("Company not found!", HttpStatusCode.NotFound);
        if (company.Users.Count(v => v.Email == accessEmail) == 0)
            throw new ErrorResponse("Access denied!", HttpStatusCode.Unauthorized);
        return company.Users.ToList();
    }

    public async Task<Company> GetUserCompany(string accessEmail)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == accessEmail);
        if (user == null) 
            throw new ErrorResponse("User not found!", HttpStatusCode.NotFound);
        return await _dbContext.Companies
            .FirstOrDefaultAsync(v => v.Users.Contains(user)) 
            ?? throw new ErrorResponse("Company not found!", HttpStatusCode.NotFound);
    }

    public async Task<bool> RemoveUserFromCompany(int id, string accessEmail)
    {
        var company = await _dbContext.Companies
            .Include(v => v.UserCreator)
            .FirstOrDefaultAsync(v => v.UserCreator!.Email == accessEmail);
        if (company == null) 
            throw new ErrorResponse("Not found company or user dont have access to company");
        company.Users.RemoveAll(v => v.Id == id);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}