using System.Threading.Tasks;

namespace ArgosDot
{
    public interface ICommand
    {
        string ActivatorCommand { get; set; }
        string ResponseText { get; set; }
        bool IsCompleted { get; set; }

        Task Run();

    }
}
