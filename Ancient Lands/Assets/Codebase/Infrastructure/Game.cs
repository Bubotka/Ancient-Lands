namespace Codebase.Infrastructure
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