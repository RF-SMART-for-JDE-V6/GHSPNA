namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgeShipConfirm
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.CollectBranchPlant = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListBranchPlants = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectShipmentNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListShipmentNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectSCAC = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListSCACs = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectTrailerNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPRONumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectSealNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ConfirmShipment = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectShipmentWeight = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListPrinters = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.WarnShipmentStatus = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.WarnSalesOrderStatus = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectShipmentUnitOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListShipmentUnitsOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
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
            // CollectSCAC
            // 
            this.CollectSCAC.ListScreen = this.ListSCACs;
            this.CollectSCAC.ListScreenControl = this.ListSCACs;
            this.CollectSCAC.ScreenName = "CollectSCAC";
            this.CollectSCAC.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSCAC_ListEvent);
            this.CollectSCAC.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSCAC_LoadEvent);
            this.CollectSCAC.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectSCAC_ValidateMethod);
            // 
            // ListSCACs
            // 
            this.ListSCACs.DataColumnForValidation = "CMSCAC";
            this.ListSCACs.ListScreenParent = this.CollectSCAC;
            this.ListSCACs.ParentScreen = this.CollectSCAC;
            this.ListSCACs.ScreenName = "ListSCACs";
            this.ListSCACs.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListSCACs_LoadEvent);
            // 
            // CollectTrailerNumber
            // 
            this.CollectTrailerNumber.ScreenName = "CollectTrailerNumber";
            this.CollectTrailerNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectTrailerNumber_LoadEvent);
            this.CollectTrailerNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectTrailerNumber_ValidateMethod);
            // 
            // CollectPRONumber
            // 
            this.CollectPRONumber.ScreenName = "CollectPRONumber";
            this.CollectPRONumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPRONumber_LoadEvent);
            this.CollectPRONumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPRONumber_ValidateMethod);
            // 
            // CollectSealNumber
            // 
            this.CollectSealNumber.ScreenName = "CollectSealNumber";
            this.CollectSealNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSealNumber_LoadEvent);
            this.CollectSealNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectSealNumber_ValidateMethod);
            // 
            // ConfirmShipment
            // 
            this.ConfirmShipment.ScreenName = "ConfirmShipment";
            this.ConfirmShipment.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ConfirmShipment_ValidateMethod);
            this.ConfirmShipment.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.ConfirmShipment_F2Method);
            // 
            // CollectShipmentWeight
            // 
            this.CollectShipmentWeight.ScreenName = "CollectShipmentWeight";
            this.CollectShipmentWeight.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectShipmentWeight_ValidateMethod);
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
            // WarnShipmentStatus
            // 
            this.WarnShipmentStatus.ScreenName = "WarnShipmentStatus";
            this.WarnShipmentStatus.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.WarnShipmentStatus_ValidateMethod);
            // 
            // WarnSalesOrderStatus
            // 
            this.WarnSalesOrderStatus.ScreenName = "WarnSalesOrderStatus";
            this.WarnSalesOrderStatus.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.WarnSalesOrderStatus_ValidateMethod);
            // 
            // CollectShipmentUnitOfMeasure
            // 
            this.CollectShipmentUnitOfMeasure.ListScreen = this.ListShipmentUnitsOfMeasure;
            this.CollectShipmentUnitOfMeasure.ListScreenControl = this.ListShipmentUnitsOfMeasure;
            this.CollectShipmentUnitOfMeasure.ScreenName = "CollectShipmentUnitOfMeasure";
            this.CollectShipmentUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShipmentUnitOfMeasure_ListEvent);
            this.CollectShipmentUnitOfMeasure.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShipmentUnitOfMeasure_LoadEvent);
            this.CollectShipmentUnitOfMeasure.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectShipmentUnitOfMeasure_ValidateMethod);
            // 
            // ListShipmentUnitsOfMeasure
            // 
            this.ListShipmentUnitsOfMeasure.DataColumnForValidation = "DRKY";
            this.ListShipmentUnitsOfMeasure.ListScreenParent = this.CollectShipmentUnitOfMeasure;
            this.ListShipmentUnitsOfMeasure.ParentScreen = this.CollectShipmentUnitOfMeasure;
            this.ListShipmentUnitsOfMeasure.ScreenName = "ListShipmentUnitsOfMeasure";
            this.ListShipmentUnitsOfMeasure.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListShipmentUnitsOfMeasure_LoadEvent);
            // 
            // StoneridgeShipConfirm
            // 
            this.StartScreen = this.CollectBranchPlant;
            this.WorkflowID = "StoneridgeShipConfirm";
        }

        #endregion

        protected GenericScreen CollectBranchPlant;

        protected GenericList ListBranchPlants;

        protected GenericScreen CollectShipmentNumber;

        protected GenericList ListShipmentNumbers;

        protected GenericScreen CollectSCAC;

        protected GenericList ListSCACs;

        protected GenericScreen CollectTrailerNumber;

        protected GenericScreen CollectPRONumber;

        protected GenericScreen CollectSealNumber;

        protected GenericScreen ConfirmShipment;

        protected GenericScreen CollectShipmentWeight;

        protected GenericScreen CollectNumberOfLabels;

        protected GenericScreen CollectPrinter;

        protected GenericList ListPrinters;

        protected GenericScreen WarnShipmentStatus;

        protected GenericScreen WarnSalesOrderStatus;

        protected GenericScreen CollectShipmentUnitOfMeasure;

        protected GenericList ListShipmentUnitsOfMeasure;
    }
}