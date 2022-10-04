using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.ShopAdministration
{
    internal class ShopAdministrationViewModel : InputsViewModel, IMenuItem
    {
        public static string TitleToMenu { get; } = "Panel administratora obszaru";
        public string Title { get; } = "Panel administratora obszaru";
        private object? _selectedObject;
        public object? SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (SetProperty(ref _selectedObject, value))
                {
                    VMShopNameChange.ResetErrorAndValues();
                    VMShopGraph.ResetErrorAndValues();
                    VMShopGraph.UpdateData();
                    VMShopGraphSetTarget.ResetErrorAndValues();
                    VMShopGraphSetTarget.UpdateData();
                    VMShopOwnerChange.ResetErrorAndValues();
                    VMShopInactivity.ResetErrorAndValues();
                }
            }
        }
        private ShopNameChangeViewModel _vMShopNameChange;
        public ShopNameChangeViewModel VMShopNameChange
        {
            get => _vMShopNameChange;
            set => SetProperty(ref _vMShopNameChange, value);
        }
        private ShopOwnerChange _vMShopOwnerChange;
        public ShopOwnerChange VMShopOwnerChange
        {
            get => _vMShopOwnerChange;
            set => SetProperty(ref _vMShopOwnerChange, value);
        }
        private ShopInactivityChangeViewModel _vMShopInactivity;
        public ShopInactivityChangeViewModel VMShopInactivity
        {
            get => _vMShopInactivity;
            set => SetProperty(ref _vMShopInactivity, value);
        }
        private ShopGraphSetTargetViewModel _vMShopGraphSetTarget;
        public ShopGraphSetTargetViewModel VMShopGraphSetTarget
        {
            get => _vMShopGraphSetTarget;
            set => SetProperty(ref _vMShopGraphSetTarget, value);
        }
        private ShopGraphViewModel _vMShopGraph;
        public ShopGraphViewModel VMShopGraph
        {
            get => _vMShopGraph;
            set => SetProperty(ref _vMShopGraph, value);
        }

        public ShopAdministrationViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            DataAssigment(managmentshopviewmodel);
            VMShopNameChange = new ShopNameChangeViewModel(managmentshopviewmodel);
            VMShopGraph = new ShopGraphViewModel(managmentshopviewmodel);
            VMShopGraphSetTarget = new ShopGraphSetTargetViewModel(managmentshopviewmodel);
            VMShopOwnerChange = new ShopOwnerChange(managmentshopviewmodel);
            VMShopInactivity = new ShopInactivityChangeViewModel(managmentshopviewmodel);
            SelectedObject = VMShopNameChange;
        }
    }
}
