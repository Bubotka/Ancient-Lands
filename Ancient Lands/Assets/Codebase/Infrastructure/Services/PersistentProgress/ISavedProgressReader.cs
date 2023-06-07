using CodeBase.Data;

namespace Codebase.Infrastructure.Services.PersistentProgress
{
  public interface ISavedProgressReader
  {
    void LoadProgress(PlayerProgress progress);
  }
}