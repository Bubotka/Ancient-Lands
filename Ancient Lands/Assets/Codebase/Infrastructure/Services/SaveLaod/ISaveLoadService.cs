using CodeBase.Data;

namespace Codebase.Infrastructure.Services.SaveLaod
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    PlayerProgress LoadProgress();
  }
}