namespace ICS.Stoneridge.Wireless.JDE.Workflow.Picking
{
    using System;
    using ICS.Database.TableObjects.RFS;
    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgePickPack
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.CollectBranchPlant = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListBranchPlants = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListPrinters = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectShipmentNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListShipmentNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectPalletNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListPalletNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListQuantityRemaining = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.ConfirmFullPallet = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfPalletLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfCartonLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPalletLabelType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.WarnInvalidSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.WarnSerialConsumed = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ExpendablePackagingWarning = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // CollectBranchPlant
            // 
            this.CollectBranchPlant.ListScreen = this.ListBranchPlants;
            this.CollectBranchPlant.ListScreenControl = this.ListBranchPlants;
            this.CollectBranchPlant.ScreenName = "CollectBranchPlant";
            this.CollectBranchPlant.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranchPlant_ListEvent);
            this.CollectBranchPlant.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranchPlant_LoadEvent);
            this.CollectBranchPlant.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectBranchPlant_ValidateMethod);
            // 
            // ListBranchPlants
            // 
            this.ListBranchPlants.DataColumnForValidation = "MCMCU";
            this.ListBranchPlants.ListScreenParent = this.CollectBranchPlant;
            this.ListBranchPlants.ParentScreen = this.CollectBranchPlant;
            this.ListBranchPlants.ScreenName = "ListBranchPlants";
            this.ListBranchPlants.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListBranchPlants_LoadEvent);
            // 
            // CollectPrinter
            // 
            this.CollectPrinter.ListScreen = this.ListPrinters;
            this.CollectPrinter.ListScreenControl = this.ListPrinters;
            this.CollectPrinter.ScreenName = "CollectPrinter";
            this.CollectPrinter.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_ListEvent);
            this.CollectPrinter.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_LoadEvent);
            this.CollectPrinter.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPrinter_ValidateMethod);
            // 
            // ListPrinters
            // 
            this.ListPrinters.DataColumnForValidation = "PrinterName";
            this.ListPrinters.ListScreenParent = this.CollectPrinter;
            this.ListPrinters.ParentScreen = this.CollectPrinter;
            this.ListPrinters.ScreenName = "ListPrinters";
            this.ListPrinters.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListPrinters_LoadEvent);
            // 
            // CollectShipmentNumber
            // 
            this.CollectShipmentNumber.ListScreen = this.ListShipmentNumbers;
            this.CollectShipmentNumber.ListScreenControl = this.ListShipmentNumbers;
            this.CollectShipmentNumber.ScreenName = "CollectShipmentNumber";
            this.CollectShipmentNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShipmentNumber_ListEvent);
            this.CollectShipmentNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectShipmentNumber_ValidateMethod);
            // 
            // ListShipmentNumbers
            // 
            this.ListShipmentNumbers.DataColumnForValidation = "XHSHPN";
            this.ListShipmentNumbers.ListScreenParent = this.CollectShipmentNumber;
            this.ListShipmentNumbers.ParentScreen = this.CollectShipmentNumber;
            this.ListShipmentNumbers.ScreenName = "ListShipmentNumbers";
            this.ListShipmentNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListShipmentNumbers_LoadEvent);
            // 
            // CollectPalletNumber
            // 
            this.CollectPalletNumber.ListScreen = this.ListPalletNumbers;
            this.CollectPalletNumber.ListScreenControl = this.ListPalletNumbers;
            this.CollectPalletNumber.ScreenName = "CollectPalletNumber";
            this.CollectPalletNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPalletNumber_ListEvent);
            this.CollectPalletNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPalletNumber_ValidateMethod);
            // 
            // ListPalletNumbers
            // 
            this.ListPalletNumbers.DataColumnForValidation = "CLCEQT";
            this.ListPalletNumbers.ListScreenParent = this.CollectPalletNumber;
            this.ListPalletNumbers.ParentScreen = this.CollectPalletNumber;
            this.ListPalletNumbers.ScreenName = "ListPalletNumbers";
            this.ListPalletNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListPalletNumbers_LoadEvent);
            // 
            // CollectSerialNumber
            // 
            this.CollectSerialNumber.ListScreen = this.ListQuantityRemaining;
            this.CollectSerialNumber.ListScreenControl = this.ListQuantityRemaining;
            this.CollectSerialNumber.ScreenName = "CollectSerialNumber";
            this.CollectSerialNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_ListEvent);
            this.CollectSerialNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_LoadEvent);
            this.CollectSerialNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectSerialNumber_ValidateMethod);
            this.CollectSerialNumber.F6Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectSerialNumber_F6Method);
            // 
            // ListQuantityRemaining
            // 
            this.ListQuantityRemaining.ListScreenParent = this.CollectSerialNumber;
            this.ListQuantityRemaining.ParentScreen = this.CollectSerialNumber;
            this.ListQuantityRemaining.ScreenName = "ListQuantityRemaining";
            this.ListQuantityRemaining.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListQuantityRemaining_EnterValidationEvent);
            this.ListQuantityRemaining.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListQuantityRemaining_LoadEvent);
            this.ListQuantityRemaining.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListQuantityRemaining_LeavingEvent);
            // 
            // ConfirmFullPallet
            // 
            this.ConfirmFullPallet.ScreenName = "ConfirmFullPallet";
            this.ConfirmFullPallet.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ConfirmFullPallet_ValidateMethod);
            // 
            // CollectNumberOfPalletLabels
            // 
            this.CollectNumberOfPalletLabels.ScreenName = "CollectNumberOfPalletLabels";
            this.CollectNumberOfPalletLabels.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectNumberOfPalletLabels_ValidateMethod);
            // 
            // CollectNumberOfCartonLabels
            // 
            this.CollectNumberOfCartonLabels.ScreenName = "CollectNumberOfCartonLabel";
            this.CollectNumberOfCartonLabels.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectNumberOfCartonLabels_ValidateMethod);
            // 
            // CollectPalletLabelType
            // 
            this.CollectPalletLabelType.ScreenName = "CollectPalletLabelType";
            this.CollectPalletLabelType.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPalletLabelType_ValidateMethod);
            // 
            // WarnInvalidSerial
            // 
            this.WarnInvalidSerial.ScreenName = "WarnInvalidSerial";
            this.WarnInvalidSerial.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.WarnInvalidSerial_ValidateMethod);
            // 
            // WarnSerialConsumed
            // 
            this.WarnSerialConsumed.ScreenName = "WarnSerialConsumed";
            this.WarnSerialConsumed.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.WarnSerialConsumed_ValidateMethod);
            // 
            // ExpendablePackagingWarning
            // 
            this.ExpendablePackagingWarning.ScreenName = "ExpendablePackagingWarning";
            this.ExpendablePackagingWarning.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ExpendablePackagingWarning_ValidateMethod);
            // 
            // StoneridgePickPack
            // 
            this.StartScreen = this.CollectBranchPlant;
            this.WorkflowID = "StoneridgePickPack";
        }

        #endregion

        protected GenericScreen CollectBranchPlant;

        protected GenericList ListBranchPlants;

        protected GenericScreen CollectPrinter;

        protected GenericList ListPrinters;

        protected GenericScreen CollectShipmentNumber;

        protected GenericList ListShipmentNumbers;

        protected GenericScreen CollectPalletNumber;

        protected GenericList ListPalletNumbers;

        protected GenericScreen CollectSerialNumber;

        protected GenericScreen ConfirmFullPallet;

        protected GenericScreen CollectNumberOfPalletLabels;

        protected GenericScreen CollectNumberOfCartonLabels;

        protected GenericScreen CollectPalletLabelType;

        protected GenericList ListQuantityRemaining;

        protected GenericScreen WarnInvalidSerial;

        protected GenericScreen WarnSerialConsumed;

        protected GenericScreen ExpendablePackagingWarning;
    }
}