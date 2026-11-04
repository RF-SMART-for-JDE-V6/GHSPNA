namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    #region Usings

    using System;
    using System.Linq;
    using ICS.Database.TableObjects.JDE;
    using ICS.Database.TableObjects.RFS;
    using ICS.Display;
    using ICS.ERPI.JDE;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    #endregion

    public partial class StoneridgeQuickRecieveLabel : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgeQuickRecieveLabelConnector _connectorWorkflow;

        public StoneridgeQuickRecieveLabelConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgeQuickRecieveLabelConnector(this)); }
        }

        #endregion

        #region Constructors and overrides

        public StoneridgeQuickRecieveLabel(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeQuickRecieveLabel()
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

        #region Screen methods

        #region Collect Branch plant

        private bool _CollectBranch_ValidateMethod(string BranchPlant)
        {
            WorkflowConnector.ValidateBranchPlant(BranchPlant);
            return true;
        }

        private void _ListBranchPlants_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadBranchPlantList(_ListBranchPlants);
        }

        #endregion

        #region Collect Printer

        private void _CollectPrinter_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadDefaultPrinter();
        }

        private bool _CollectPrinter_ValidateMethod(string Printer)
        {
            WorkflowConnector.ValidatePrinter(Printer);
            return true;
        }

        private void _ListPrinters_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadPrinterList(_ListPrinters);
        }

        #endregion

        #region CollectShipment

        private bool _CollectShipment_ValidateMethod(string ShipmentNumber)
        {
            WorkflowConnector.ValidateShipmentNumber(ShipmentNumber);
            return true;
        }

        private void _ListShipmentNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadShipmentNumberList(_ListShipmentNumbers);
        }

        #endregion

        #region CollectPallet

        private bool _CollectPallet_ValidateMethod(string PalletNumber)
        {
            WorkflowConnector.ValidatePalletNumber(PalletNumber);
            return true;
        }

        private void _ListPalletNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadPalletNumberList(_ListPalletNumbers);
        }

        #endregion

        #region Print Label

        private bool _PrintLabel_ValidateMethod(string PrintLabel)
        {
            WorkflowConnector.ValidatePrintLabel(PrintLabel);

            RestartLoop(_CollectShipment);
            return true;
        }

        #endregion

        #endregion

        private bool CollectNumberOfCartonLabels_ValidateMethod(string NumberOfCartonLabels)
        {
            WorkflowConnector.ValidateNumberOfCartonLabels(NumberOfCartonLabels);

            return true;
        }
    }
}