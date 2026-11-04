namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    using System;
    using ICS.Database.TableObjects.RFS;
    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgeQuickRecieveLabel
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this._CollectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ListBranchPlants = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ListPrinters = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectShipment = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ListShipmentNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._PrintLabel = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectPallet = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ListPalletNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectNumberOfCartonLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _CollectBranch
            // 
            this._CollectBranch.ListScreen = this._ListBranchPlants;
            this._CollectBranch.ListScreenControl = this._ListBranchPlants;
            this._CollectBranch.NextScreen = this._CollectPrinter;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._CollectBranch_ValidateMethod);
            // 
            // _ListBranchPlants
            // 
            this._ListBranchPlants.DataColumnForValidation = "MCMCU";
            this._ListBranchPlants.ListScreenParent = this._CollectBranch;
            this._ListBranchPlants.ParentScreen = this._CollectBranch;
            this._ListBranchPlants.ScreenName = "ListBranchPlants";
            this._ListBranchPlants.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ListBranchPlants_LoadEvent);
            // 
            // _CollectPrinter
            // 
            this._CollectPrinter.ListScreen = this._ListPrinters;
            this._CollectPrinter.ListScreenControl = this._ListPrinters;
            this._CollectPrinter.NextScreen = this._CollectShipment;
            this._CollectPrinter.ScreenName = "CollectPrinter";
            this._CollectPrinter.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectPrinter_LoadEvent);
            this._CollectPrinter.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._CollectPrinter_ValidateMethod);
            // 
            // _ListPrinters
            // 
            this._ListPrinters.DataColumnForValidation = "PrinterName";
            this._ListPrinters.ListScreenParent = this._CollectPrinter;
            this._ListPrinters.ParentScreen = this._CollectPrinter;
            this._ListPrinters.ScreenName = "ListPrinters";
            this._ListPrinters.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ListPrinters_LoadEvent);
            // 
            // _CollectShipment
            // 
            this._CollectShipment.ListScreen = this._ListShipmentNumbers;
            this._CollectShipment.ListScreenControl = this._ListShipmentNumbers;
            this._CollectShipment.NextScreen = this.CollectNumberOfCartonLabels;
            this._CollectShipment.ScreenName = "CollectShipment";
            this._CollectShipment.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._CollectShipment_ValidateMethod);
            // 
            // _ListShipmentNumbers
            // 
            this._ListShipmentNumbers.DataColumnForValidation = "CDSHPN";
            this._ListShipmentNumbers.ListScreenParent = this._CollectShipment;
            this._ListShipmentNumbers.ParentScreen = this._CollectShipment;
            this._ListShipmentNumbers.ScreenName = "ListShipmentNumbers";
            this._ListShipmentNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ListShipmentNumbers_LoadEvent);
            // 
            // _PrintLabel
            // 
            this._PrintLabel.ScreenName = "PrintLabel";
            this._PrintLabel.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._PrintLabel_ValidateMethod);
            // 
            // _CollectPallet
            // 
            this._CollectPallet.ListScreen = this._ListPalletNumbers;
            this._CollectPallet.ListScreenControl = this._ListPalletNumbers;
            this._CollectPallet.NextScreen = this._PrintLabel;
            this._CollectPallet.ScreenName = "CollectPallet";
            this._CollectPallet.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._CollectPallet_ValidateMethod);
            // 
            // _ListPalletNumbers
            // 
            this._ListPalletNumbers.DataColumnForValidation = "CDURRF";
            this._ListPalletNumbers.ListScreenParent = this._CollectPallet;
            this._ListPalletNumbers.ParentScreen = this._CollectPallet;
            this._ListPalletNumbers.ScreenName = "ListPalletNumbers";
            this._ListPalletNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ListPalletNumbers_LoadEvent);
            // 
            // CollectNumberOfCartonLabels
            // 
            this.CollectNumberOfCartonLabels.NextScreen = this._PrintLabel;
            this.CollectNumberOfCartonLabels.ScreenName = "CollectNumberOfCartonLabels";
            this.CollectNumberOfCartonLabels.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectNumberOfCartonLabels_ValidateMethod);
            // 
            // StoneridgeQuickRecieveLabel
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeQuickRecieveLabel";
        }

        #endregion

        protected GenericScreen _CollectBranch;

        protected GenericScreen _CollectPrinter;

        protected GenericScreen _CollectShipment;

        protected GenericScreen _CollectPallet;

        protected GenericScreen _PrintLabel;

        protected GenericList _ListBranchPlants;

        protected GenericList _ListPrinters;

        protected GenericList _ListShipmentNumbers;

        protected GenericList _ListPalletNumbers;

        protected GenericScreen CollectNumberOfCartonLabels;
    }
}