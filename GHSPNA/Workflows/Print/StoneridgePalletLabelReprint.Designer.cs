namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    using System;
    using ICS.Database.TableObjects.RFS;
    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgePalletLabelReprint
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
            this.CollectLabelType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // CollectBranchPlant
            // 
            this.CollectBranchPlant.ListScreen = this.ListBranchPlants;
            this.CollectBranchPlant.ListScreenControl = this.ListBranchPlants;
            this.CollectBranchPlant.ScreenName = "CollectBranchPlant";
            this.CollectBranchPlant.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranchPlant_ListEvent);
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
            this.ListShipmentNumbers.DataColumnForValidation = "CDSHPN";
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
            this.ListPalletNumbers.DataColumnForValidation = "CDURRF";
            this.ListPalletNumbers.ListScreenParent = this.CollectPalletNumber;
            this.ListPalletNumbers.ParentScreen = this.CollectPalletNumber;
            this.ListPalletNumbers.ScreenName = "ListPalletNumbers";
            this.ListPalletNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListPalletNumbers_LoadEvent);
            // 
            // CollectLabelType
            // 
            this.CollectLabelType.ScreenName = "CollectLabelType";
            this.CollectLabelType.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectLabelType_ValidateMethod);
            // 
            // CollectNumberOfLabels
            // 
            this.CollectNumberOfLabels.ScreenName = "CollectNumberOfLabels";
            this.CollectNumberOfLabels.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectNumberOfLabels_LoadEvent);
            this.CollectNumberOfLabels.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectNumberOfLabels_ValidateMethod);
            // 
            // StoneridgePalletLabelReprint
            // 
            this.StartScreen = this.CollectBranchPlant;
            this.WorkflowID = "StoneridgePalletLabelReprint";
        }

        #endregion

        protected GenericScreen CollectBranchPlant;

        protected GenericList ListBranchPlants;

        protected GenericScreen CollectPrinter;

        protected GenericList ListPrinters;

        protected GenericScreen CollectShipmentNumber;

        protected GenericList ListShipmentNumbers;

        protected GenericScreen CollectPalletNumber;

        protected GenericScreen CollectLabelType;

        protected GenericScreen CollectNumberOfLabels;

        protected GenericList ListPalletNumbers;
    }
}