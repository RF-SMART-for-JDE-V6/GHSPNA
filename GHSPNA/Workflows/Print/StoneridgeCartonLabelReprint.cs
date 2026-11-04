using ICS.Display;
using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    public partial class StoneridgeCartonLabelReprint : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgeCartonLabelReprintConnector _connectorWorkflow;

        public StoneridgeCartonLabelReprintConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgeCartonLabelReprintConnector(this)); }
        }

        #endregion

        #region Constructors and Overrides

        public StoneridgeCartonLabelReprint(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeCartonLabelReprint()
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

            Next(CollectSerialNumber);

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

        #region Collect Serial Number

        private bool CollectSerialNumber_ValidateMethod(string SerialNumber)
        {
            WorkflowConnector.ValidateSerialNumber(SerialNumber);

            Next(CollectNumberOfLabels);

            return true;
        }

        private void CollectSerialNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListSerialNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadSerialNumberList(ListSerialNumbers);
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

            WorkflowConnector.PrintBarcodeLabels();

            RestartLoop(CollectSerialNumber);

            return true;
        }

        #endregion

        #endregion
    }
}