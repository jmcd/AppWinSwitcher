namespace AppWinSwitcher;

internal class Program : IDisposable
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        new Program().Run();
    }
    
    private readonly MainForm _mainForm;
    private readonly Controller _controller;

    private Program()
    {
        _mainForm = new MainForm();
        _controller = new Controller(_mainForm);
    }
    
    private void Run()
    {
        Application.Run(_mainForm);
    }

    public void Dispose()
    {
        _controller.Dispose();
    }
}