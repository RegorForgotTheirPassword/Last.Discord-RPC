using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using LastfmDiscordRPC2.DataTypes;
using LastfmDiscordRPC2.IO;
using LastfmDiscordRPC2.ViewModels.Update;
using LastfmDiscordRPC2.Views;
using ReactiveUI;

namespace LastfmDiscordRPC2.ViewModels.Controls;

public class PreviewConfigControlViewModel : AbstractControlViewModel, ISettableViewModel
{
    private readonly SaveCfgIOService _saveCfgIOService;
    public override string Name => "PreviewConfigControl";

    private string _details = Empty;
    private string _state = Empty;
    private string _largeImageLabel = Empty;
    private string _smallImageLabel = Empty;
    private readonly DialogWindow _window;
    private ObservableCollection<PreviewButton> _buttons = new ObservableCollection<PreviewButton>();

    public string Details
    {
        get => _details;
        set => this.RaiseAndSetIfChanged(ref _details, value);
    }

    public string State
    {
        get => _state;
        set => this.RaiseAndSetIfChanged(ref _state, value);
    }

    public string LargeImageLabel
    {
        get => _largeImageLabel;
        set => this.RaiseAndSetIfChanged(ref _largeImageLabel, value);
    }

    public string SmallImageLabel
    {
        get => _smallImageLabel;
        set => this.RaiseAndSetIfChanged(ref _smallImageLabel, value);
    }

    public ObservableCollection<PreviewButton> Buttons
    {
        get => _buttons;
        set => this.RaiseAndSetIfChanged(ref _buttons, value);
    }

    public bool CanSave => Buttons.All(button => !button.IsInvalidUrl);
    
    public UIContext Context { get; }
    public ReactiveCommand<Unit, Unit> SavePreviewCmd { get; }
    public ReactiveCommand<Unit, Unit> ResetPreviewCmd { get; }
    public ReactiveCommand<Unit, Unit> VarDialogCmd { get; }

    public PreviewConfigControlViewModel(UIContext context, SaveCfgIOService saveCfgIOService, DialogWindow window)
    {
        _saveCfgIOService = saveCfgIOService;
        _window = window;
        Context = context;

        SavePreviewCmd = ReactiveCommand.Create(SavePreview);
        ResetPreviewCmd = ReactiveCommand.Create(ResetPreview);
        VarDialogCmd = ReactiveCommand.Create(VarDialog);
        
        SetProperties();
    }
    
    private void VarDialog()
    {
        DialogWindow.Show(null);
    }

    private void SavePreview()
    {
        _saveCfgIOService.SaveCfg.UserRPCCfg.Details = Details;
        _saveCfgIOService.SaveCfg.UserRPCCfg.State = State;
        _saveCfgIOService.SaveCfg.UserRPCCfg.LargeImageLabel = LargeImageLabel;
        _saveCfgIOService.SaveCfg.UserRPCCfg.SmallImageLabel = SmallImageLabel;
        _saveCfgIOService.SaveConfigData();
    }

    private void ResetPreview()
    {
        Details = SaveVars.DefaultDetails;
        State = SaveVars.DefaultState;
        LargeImageLabel = SaveVars.DefaultLargeImageLabel;
        SmallImageLabel = SaveVars.DefaultSmallImageLabel;
        SavePreview();
    }

    public void SetProperties()
    {
        Details = _saveCfgIOService.SaveCfg.UserRPCCfg.Details;
        State = _saveCfgIOService.SaveCfg.UserRPCCfg.State;
        LargeImageLabel = _saveCfgIOService.SaveCfg.UserRPCCfg.LargeImageLabel;
        SmallImageLabel = _saveCfgIOService.SaveCfg.UserRPCCfg.SmallImageLabel;
        Buttons = new ObservableCollection<PreviewButton>(
            _saveCfgIOService.SaveCfg.UserRPCCfg.UserButtons.Select(button =>
                new PreviewButton(() => this.RaisePropertyChanged(nameof(Buttons)))
                {
                    URL = button.URL,
                    Label = button.Label
                }
            )
        );
    }
}