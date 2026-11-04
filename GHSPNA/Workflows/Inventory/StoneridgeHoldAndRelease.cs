namespace ICS.Stoneridge.Wireless.JDE.Workflow.Inventory
{
    #region Usings

    using ICS.Display;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    #endregion

    public partial class StoneridgeHoldAndRelease : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgeHoldAndReleaseConnector _connectorWorkflow;

        public StoneridgeHoldAndReleaseConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgeHoldAndReleaseConnector(this)); }
        }

        #endregion

        #region Constructors and Overrides

        public StoneridgeHoldAndRelease(UserSession userSession) : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeHoldAndRelease()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
        }

        public override void Dispose()
        {
            ScreenMap.ClearAllWorkflows();
            base.Dispose();
        }

        #endregion

        #region Screen Methods

        #region Collect Branch Plant

        private bool CollectBranchPlant_ValidateMethod(string BranchPlant)
        {
            WorkflowConnector.ValidateBranchPlant(BranchPlant);

            Next(CollectExplanation);

            return true;
        }

        private void CollectBranchPlant_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListBranchPlants_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadBranchPlantList(ListBranchPlants);
        }

        #endregion

        #region Collect Explanation

        private bool CollectExplanation_ValidateMethod(string Explanation)
        {
            WorkflowConnector.ValidateExplanation(Explanation);

            Next(CollectReasonCode);

            return true;
        }

        #endregion

        #region Collect Reason Code

        private bool CollectReasonCode_ValidateMethod(string ReasonCode)
        {
            WorkflowConnector.ValidateReasonCode(ReasonCode);

            Next(CollectItemNumber);

            return true;
        }

        private void CollectReasonCode_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListReasonCodes_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadReasonCodeList(ListReasonCodes);
        }

        #endregion

        #region Collect Item Number

        private bool CollectItemNumber_ValidateMethod(string ItemNumber)
        {
            WorkflowConnector.ValidateItemNumber(ItemNumber);

            if (WorkflowConnector.ItemIsLotControlled())
            {
                Next(CollectLotSerialNumber);
            }
            else
            {
                WorkflowConnector.ClearLotDetails();
                Next(CollectFromLocation);
            }

            return true;
        }

        private void CollectItemNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListItemNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadItemNumberList(ListItemNumbers);
        }

        #endregion

        #region Collect Lot / Serial Number

        private bool CollectLotSerialNumber_ValidateMethod(string LotSerialNumber)
        {
            WorkflowConnector.ValidateLotSerialNumber(LotSerialNumber);

            Next(CollectFromLocation);

            return true;
        }

        #endregion

        #region Collect From Location

        private bool CollectFromLocation_ValidateMethod(string FromLocation)
        {
            WorkflowConnector.ValidateFromLocation(FromLocation);

            Next(CollectToLocation);

            return true;
        }

        #endregion

        #region Collect To Location

        private bool CollectToLocation_ValidateMethod(string ToLocation)
        {
            WorkflowConnector.ValidateToLocation(ToLocation);

            Next(CollectQuantity);

            return true;
        }

        #endregion

        #region Collect Quantity

        private bool CollectQuantity_ValidateMethod(string Quantity)
        {
            WorkflowConnector.ValidateQuantity(Quantity);

            Next(CollectUnitOfMeasure);

            return true;
        }

        #endregion

        #region Collect Unit Of Measure

        private void CollectUnitOfMeasure_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadDefaultUnitOfMeasure();
        }

        private bool CollectUnitOfMeasure_ValidateMethod(string UnitOfMeasure)
        {
            WorkflowConnector.ValidateUnitOfMeasure(UnitOfMeasure);

            WorkflowConnector.SendToERP();

            Next(CollectNumberOfLabels);

            return true;
        }

        #endregion

        #region Collect Number Of Labels

        private bool CollectNumberOfLabels_ValidateMethod(string NumberOfLabels)
        {
            WorkflowConnector.ValidateNumberOfLabels(NumberOfLabels);

            Next(CollectPrinter);

            return true;
        }

        #endregion

        #region Collect Printer

        private void CollectPrinter_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadDefaultPrinter();
        }

        private bool CollectPrinter_ValidateMethod(string Printer)
        {
            WorkflowConnector.ValidatePrinter(Printer);

            RestartLoop(CollectBranchPlant);

            return true;
        }

        private void CollectPrinter_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListPrinters_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadPrinterList(ListPrinters);
        }

        #endregion

        #endregion
    }
}