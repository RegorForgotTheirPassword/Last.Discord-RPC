using ReactiveUI;

namespace LastfmDiscordRPC2.ViewModels.Panes;

public class HomeViewModel : ReactiveObject, IPaneViewModel
{
    public string PaneName { get => "Home"; }
}