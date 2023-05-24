using Codebase.Data;

namespace Codebase.Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressService:IService 
    {
        PlayerProgress Progress { get; set; }
    }
}