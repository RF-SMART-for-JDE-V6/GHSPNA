using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    public partial class StoneridgeLabelPrint
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.CollectBranchPlant = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DefinedPrintersList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectItemNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ItemMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectQtyOnLabel = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectContainer = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectUOM = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
            this.UOMConversionsList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectNumberOfCartons = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPurchaseOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectDateReceived = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectRevision = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectDeliverTo = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectInspection = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLotSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // CollectBranchPlant
            // 
            this.CollectBranchPlant.ListScreen = this.BranchPlantList;
            this.CollectBranchPlant.ListScreenControl = this.BranchPlantList;
            this.CollectBranchPlant.NextScreen = this.CollectPrinter;
            this.CollectBranchPlant.ScreenName = "CollectBranchPlant";
            this.CollectBranchPlant.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectBranchPlant_ValidateMethod);
            // 
            // BranchPlantList
            // 
            this.BranchPlantList.DataColumnForValidation = "MCMCU";
            this.BranchPlantList.ListScreenParent = this.CollectBranchPlant;
            this.BranchPlantList.ParentScreen = this.CollectBranchPlant;
            this.BranchPlantList.ScreenName = "F0006_List";
            this.BranchPlantList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_EnterValidationEvent);
            this.BranchPlantList.LoadingMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.LoadingHandler(this.BranchPlantList_LoadingMethod);
            // 
            // CollectPrinter
            // 
            this.CollectPrinter.ListScreen = this._DefinedPrintersList;
            this.CollectPrinter.ListScreenControl = this._DefinedPrintersList;
            this.CollectPrinter.NextScreen = this.CollectItemNumber;
            this.CollectPrinter.ScreenName = "CollectPrinter";
            this.CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterValidationEvent);
            this.CollectPrinter.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_LoadEvent);
            this.CollectPrinter.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPrinter_ValidateMethod);
            // 
            // _DefinedPrintersList
            // 
            this._DefinedPrintersList.ListScreenParent = this.CollectPrinter;
            this._DefinedPrintersList.ParentScreen = this.CollectPrinter;
            this._DefinedPrintersList.ScreenName = "FRFS_Printers";
            this._DefinedPrintersList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersList_EnterValidationEvent);
            this._DefinedPrintersList.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersList_BackValidationEvent);
            this._DefinedPrintersList.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersList_BackValidationEvent);
            this._DefinedPrintersList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersList_LoadEvent);
            // 
            // CollectItemNumber
            // 
            this.CollectItemNumber.BackScreen = this.CollectPrinter;
            this.CollectItemNumber.ListScreen = this._ItemMasterList;
            this.CollectItemNumber.ListScreenControl = this._ItemMasterList;
            this.CollectItemNumber.NextScreen = this.CollectQtyOnLabel;
            this.CollectItemNumber.ScreenName = "CollectItemNumber";
            this.CollectItemNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_EnterValidationEvent);
            this.CollectItemNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_LoadEvent);
            // 
            // _ItemMasterList
            // 
            this._ItemMasterList.ListScreenParent = this.CollectItemNumber;
            this._ItemMasterList.ParentScreen = this.CollectItemNumber;
            this._ItemMasterList.ScreenName = "F4101_List";
            this._ItemMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ItemMasterList_EnterValidationEvent);
            this._ItemMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ItemMasterList_LoadEvent);
            // 
            // CollectQtyOnLabel
            // 
            this.CollectQtyOnLabel.NextScreen = this.CollectContainer;
            this.CollectQtyOnLabel.ScreenName = "CollectQtyOnLabel";
            this.CollectQtyOnLabel.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQtyOnLabel_LoadEvent);
            this.CollectQtyOnLabel.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectQtyOnLabel_ValidateMethod);
            // 
            // CollectContainer
            // 
            this.CollectContainer.ScreenName = "CollectContainer";
            this.CollectContainer.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectContainer_LoadEvent);
            this.CollectContainer.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectContainer_ValidateMethod);
            // 
            // CollectNumberOfLabels
            // 
            this.CollectNumberOfLabels.ScreenName = "CollectNumberOfLabels";
            this.CollectNumberOfLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectNumberOfLabels_EnterValidationEvent);
            // 
            // CollectUOM
            // 
            this.CollectUOM.BranchScreen = null;
            this.CollectUOM.DisableToleranceCheck = true;
            this.CollectUOM.DualQtyScreen = null;
            this.CollectUOM.ItemScreen = null;
            this.CollectUOM.ListScreen = this.UOMConversionsList;
            this.CollectUOM.ListScreenControl = this.UOMConversionsList;
            this.CollectUOM.OriginalQtyScreen = null;
            this.CollectUOM.OriginalUOMScreen = null;
            this.CollectUOM.ScreenName = "CollectUOM";
            this.CollectUOM.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUOM_EnterValidationEvent);
            this.CollectUOM.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUOM_LoadEvent);
            this.CollectUOM.UOMConversionsList.ScreenName = "F41002_List";
            // 
            // UOMConversionsList
            // 
            this.UOMConversionsList.ListScreenParent = this.CollectUOM;
            this.UOMConversionsList.ParentScreen = this.CollectUOM;
            this.UOMConversionsList.ScreenName = "F41002_List";
            this.UOMConversionsList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UOMConversionList_EnterValidationEvent);
            this.UOMConversionsList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UOMConversionList_LoadEvent);
            // 
            // CollectNumberOfCartons
            // 
            this.CollectNumberOfCartons.NextScreen = this.CollectItemNumber;
            this.CollectNumberOfCartons.ScreenName = "CollectNumberOfCartons";
            this.CollectNumberOfCartons.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectNumberOfCartons_EnterValidationEvent);
            this.CollectNumberOfCartons.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectNumberOfCartons_LoadEvent);
            // 
            // CollectPurchaseOrder
            // 
            this.CollectPurchaseOrder.ScreenName = "CollectPurchaseOrder";
            this.CollectPurchaseOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPurchaseOrder_EnterValidationEvent);
            // 
            // CollectDateReceived
            // 
            this.CollectDateReceived.ScreenName = "CollectDateReceived";
            this.CollectDateReceived.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDateReceived_EnterValidationEvent);
            this.CollectDateReceived.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDateReceived_LoadEvent);
            // 
            // CollectRevision
            // 
            this.CollectRevision.ScreenName = "CollectRevision";
            this.CollectRevision.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectRevision_EnterValidationEvent);
            // 
            // CollectDeliverTo
            // 
            this.CollectDeliverTo.ScreenName = "CollectDeliverTo";
            this.CollectDeliverTo.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDeliverTo_EnterValidationEvent);
            // 
            // CollectInspection
            // 
            this.CollectInspection.ScreenName = "CollectInspection";
            this.CollectInspection.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectInspection_EnterValidationEvent);
            this.CollectInspection.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectInspection_LoadEvent);
            // 
            // CollectLocation
            // 
            this.CollectLocation.ScreenName = "CollectLocation";
            this.CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterValidationEvent);
            // 
            // CollectLotSerial
            // 
            this.CollectLotSerial.ScreenName = "CollectLotSerial";
            this.CollectLotSerial.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectLotSerial_ValidateMethod);
            this.CollectLotSerial.NextScreen = CollectQtyOnLabel;
            // 
            // StoneridgeLabelPrint
            // 
            this.StartScreen = this.CollectBranchPlant;
            this.WorkflowID = "StoneridgeLabelPrint";
        }

        #endregion

        protected GenericScreen CollectBranchPlant;

        protected GenericScreen CollectItemNumber;

        protected GenericScreen CollectQtyOnLabel;

        protected GenericScreen CollectNumberOfLabels;

        protected GenericList BranchPlantList;

        protected GenericList _ItemMasterList;

        protected GenericScreen CollectPrinter;

        protected GenericList _DefinedPrintersList;

        protected GenericScreen CollectContainer;

        protected ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen CollectUOM;

        protected GenericList UOMConversionsList;

        protected GenericScreen CollectNumberOfCartons;

        protected GenericScreen CollectPurchaseOrder;

        protected GenericScreen CollectDateReceived;

        protected GenericScreen CollectRevision;

        protected GenericScreen CollectDeliverTo;

        protected GenericScreen CollectInspection;

        protected GenericScreen CollectLocation;

        private GenericScreen CollectLotSerial;
    }
}