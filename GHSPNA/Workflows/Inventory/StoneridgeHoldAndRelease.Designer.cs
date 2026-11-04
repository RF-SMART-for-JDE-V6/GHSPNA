namespace ICS.Stoneridge.Wireless.JDE.Workflow.Inventory
{
    #region Usings

    using ICS.Workflow.Base.WorkflowDesign.Components;

    #endregion

    public partial class StoneridgeHoldAndRelease
    {
        #region Screen Definitions

        protected GenericScreen CollectExplanation;

        protected GenericScreen CollectReasonCode;

        protected GenericScreen CollectItemNumber;

        protected GenericScreen CollectLotSerialNumber;

        protected GenericScreen CollectFromLocation;

        protected GenericScreen CollectToLocation;

        protected GenericScreen CollectQuantity;

        protected GenericScreen CollectUnitOfMeasure;

        protected GenericScreen CollectNumberOfLabels;

        protected GenericScreen CollectPrinter;

        protected GenericList ListReasonCodes;

        protected GenericList ListItemNumbers;

        protected GenericList ListPrinters;

        protected GenericScreen CollectBranchPlant;

        protected GenericList ListBranchPlants;

        #endregion

        #region Component Designer Generated Code

        private void InitializeComponent()
        {
            this.CollectExplanation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectReasonCode = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListReasonCodes = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectItemNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListItemNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectLotSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectFromLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectToLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectQuantity = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectUnitOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListPrinters = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectBranchPlant = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListBranchPlants = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            // 
            // CollectExplanation
            // 
            this.CollectExplanation.ScreenName = "CollectExplanation";
            this.CollectExplanation.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectExplanation_ValidateMethod);
            // 
            // CollectReasonCode
            // 
            this.CollectReasonCode.ListScreen = this.ListReasonCodes;
            this.CollectReasonCode.ListScreenControl = this.ListReasonCodes;
            this.CollectReasonCode.ScreenName = "CollectReasonCode";
            this.CollectReasonCode.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectReasonCode_ListEvent);
            this.CollectReasonCode.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectReasonCode_ValidateMethod);
            // 
            // ListReasonCodes
            // 
            this.ListReasonCodes.DataColumnForValidation = "DRKY";
            this.ListReasonCodes.ListScreenParent = this.CollectReasonCode;
            this.ListReasonCodes.ParentScreen = this.CollectReasonCode;
            this.ListReasonCodes.ScreenName = "ListReasonCodes";
            this.ListReasonCodes.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListReasonCodes_LoadEvent);
            // 
            // CollectItemNumber
            // 
            this.CollectItemNumber.ListScreen = this.ListItemNumbers;
            this.CollectItemNumber.ListScreenControl = this.ListItemNumbers;
            this.CollectItemNumber.ScreenName = "CollectItemNumber";
            this.CollectItemNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_ListEvent);
            this.CollectItemNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectItemNumber_ValidateMethod);
            // 
            // ListItemNumbers
            // 
            this.ListItemNumbers.DataColumnForValidation = "IMLITM";
            this.ListItemNumbers.ListScreenParent = this.CollectItemNumber;
            this.ListItemNumbers.ParentScreen = this.CollectItemNumber;
            this.ListItemNumbers.ScreenName = "ListItemNumbers";
            this.ListItemNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListItemNumbers_LoadEvent);
            // 
            // CollectLotSerialNumber
            // 
            this.CollectLotSerialNumber.ScreenName = "CollectLotSerialNumber";
            this.CollectLotSerialNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectLotSerialNumber_ValidateMethod);
            // 
            // CollectFromLocation
            // 
            this.CollectFromLocation.ScreenName = "CollectFromLocation";
            this.CollectFromLocation.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectFromLocation_ValidateMethod);
            // 
            // CollectToLocation
            // 
            this.CollectToLocation.ScreenName = "CollectToLocation";
            this.CollectToLocation.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectToLocation_ValidateMethod);
            // 
            // CollectQuantity
            // 
            this.CollectQuantity.ScreenName = "CollectQuantity";
            this.CollectQuantity.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectQuantity_ValidateMethod);
            // 
            // CollectUnitOfMeasure
            // 
            this.CollectUnitOfMeasure.ScreenName = "CollectUnitOfMeasure";
            this.CollectUnitOfMeasure.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUnitOfMeasure_LoadEvent);
            this.CollectUnitOfMeasure.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectUnitOfMeasure_ValidateMethod);
            // 
            // CollectNumberOfLabels
            // 
            this.CollectNumberOfLabels.ScreenName = "CollectNumberOfLabels";
            this.CollectNumberOfLabels.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectNumberOfLabels_ValidateMethod);
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
            // StoneridgeHoldAndRelease
            // 
            this.StartScreen = this.CollectBranchPlant;
            this.WorkflowID = "StoneridgeHoldAndRelease";
        }

        #endregion
    }
}