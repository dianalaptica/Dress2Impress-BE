using Dress2Impress.DataAccess;

namespace Dress2Impress.BusinessLogic.Services;

public abstract class BaseService
{
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }   
}
