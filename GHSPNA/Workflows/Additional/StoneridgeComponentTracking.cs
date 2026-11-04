using ICS.Display;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.Database.TableObjects.JDE;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    public partial class StoneridgeComponentTracking : RFSWorkflowDesigner
    {
        #region Class Level defs

        #region Tables

        private F0006 _BranchPlantTable;

        private F4101 _ItemMasterTable;

        #endregion

        #endregion

        #region Business connector

        private StoneridgeComponentTrackingConnector connectorWorkflow;

        public StoneridgeComponentTrackingConnector _WorkflowConnector
        {
            get { return connectorWorkflow ?? (connectorWorkflow = new StoneridgeComponentTrackingConnector(this)); }
        }

        #endregion

        #region Constructors and overrides

        public StoneridgeComponentTracking(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeComponentTracking()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
            #region Table

            this._BranchPlantTable = new F0006(UserSession.DataAccess, "");
            this._ItemMasterTable = new F4101(UserSession.DataAccess, "");

            #endregion
        }

        public override void Dispose()
        {
            ScreenMap.ClearAllWorkflows();
            base.Dispose();
        }

        #endregion

        private void BranchPlantList_LoadingMethod(ScreenEventArgs e)
        {
            BranchPlantList.Table = _WorkflowConnector._BranchPlantTable;
            _WorkflowConnector._BranchPlantTable.FetchBranchPlants();
        }
    }
}