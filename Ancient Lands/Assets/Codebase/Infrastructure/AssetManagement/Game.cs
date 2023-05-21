namespace Codebase.Infrastructure.AssetManagement
{
    public  class Game
    {
        public static InputService InputService;

        public  Game()
        {
            InputService = new InputService();
        }
    }
}