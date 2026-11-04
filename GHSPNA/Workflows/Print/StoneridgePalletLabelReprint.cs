namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    #region Usings

    using ICS.Display;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    #endregion

    public partial class StoneridgePalletLabelReprint : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgePalletLabelReprintConnector _connectorWorkflow;

        public StoneridgePalletLabelReprintConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgePalletLabelReprintConnector(this)); }
        }

        #endregion

        #region Constructors and Overrides

        public StoneridgePalletLabelReprint(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgePalletLabelReprint()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
        }

        public override void Dispose()
        {
            ScreenMap.ClearAllWorkflows();
            WorkflowConnector.Dispose();
            base.Dispose();
        }

        #endregion

        #region Screen Methods

        #region Collect Branch Plant

        private bool CollectBranchPlant_ValidateMethod(string BranchPlant)
        {
            WorkflowConnector.ValidateBranchPlant(BranchPlant);

            Next(CollectPrinter);

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

        #region Collect Printer

        private void CollectPrinter_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadDefaultPrinter();
        }

        private bool CollectPrinter_ValidateMethod(string Printer)
        {
            WorkflowConnector.ValidatePrinter(Printer);

            Next(CollectShipmentNumber);

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

        #region Collect Shipment Number

        private bool CollectShipmentNumber_ValidateMethod(string ShipmentNumber)
        {
            WorkflowConnector.ValidateShipmentNumber(ShipmentNumber);

            Next(CollectPalletNumber);

            return true;
        }

        private void CollectShipmentNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListShipmentNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadShipmentNumberList(ListShipmentNumbers);
        }

        #endregion

        #region Collect Pallet Number

        private bool CollectPalletNumber_ValidateMethod(string PalletNumber)
        {
            WorkflowConnector.ValidatePalletNumber(PalletNumber);

            if (WFOptions.RetrieveOptionValue("PrintMultipleMaster") == "Y")
            {
                WorkflowConnector.ValidateLabelType("M");
                Next(CollectNumberOfLabels);
            }
            else
            {
                Next(CollectLabelType);
            }

            return true;
        }

        private void CollectPalletNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListPalletNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadPalletNumberList(ListPalletNumbers);
        }

        #endregion

        #region Collect Label Type

        private bool CollectLabelType_ValidateMethod(string LabelType)
        {
            WorkflowConnector.ValidateLabelType(LabelType);

            Next(CollectNumberOfLabels);

            return true;
        }

        #endregion

        #region Collect Number Of Labels

        private void CollectNumberOfLabels_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadDefaultNumberOfLabels();
        }

        private bool CollectNumberOfLabels_ValidateMethod(string NumberOfLabels)
        {
            WorkflowConnector.ValidaterNumberOfLabels(NumberOfLabels);
            if (WFOptions.RetrieveOptionValue("PrintMultipleMaster") == "Y")
            {
                WorkflowConnector.PrintBarcodeLabelsPallet();
                RestartLoop(CollectPalletNumber);
            }
            else
            {
                WorkflowConnector.PrintBarcodeLabels();
                RestartLoop(CollectShipmentNumber);
            }

            return true;
        }

        #endregion

        #endregion
    }
}